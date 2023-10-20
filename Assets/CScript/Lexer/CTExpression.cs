using System.Collections.Generic;
using Riten.CScript.Lexer;

[System.Serializable]
public class CTExpression : CTTypedNode
{
    public readonly CTTypedNode TreeRoot;

    public CTExpression(CTTypedNode tree)
    {
        TreeRoot = tree;
        AddChild(tree);
    }
    
    public static int TokenPriority(CToken token)
    {
        if (token.Options == CTOptions.UNARY)
            return 2;
        
        switch (token.Type)
        {
            case CTokenType.MULTIPLY:
            case CTokenType.DIVIDE:
                return 1;
        }

        return 0;
    }

    public static CTNode Parse(CTLexer lexer, string hint)
    {
        var valueStack = new Stack<CTTypedNode>();
        var operatorStack = new Stack<CToken>();

        valueStack.Clear();
        operatorStack.Clear();
        
        bool wasOperator = true;
        int parenthesisCount = 0;
        
        while (!lexer.IsEndOfFile())
        {
            var tokenType = lexer.Peek().Type;
            
            if (tokenType is CTokenType.SEMICOLON or CTokenType.COMMA)
                break;

            if (tokenType == CTokenType.RIGHT_PARENTHESES && parenthesisCount <= 0)
                break;

            switch (tokenType)
            {
                case CTokenType.NUMBER:
                case CTokenType.WORD:
                    if (wasOperator == false)
                        throw new CTLexerException(lexer.Peek(), $"Missing operator for '{lexer.Peek().Span}'.");
                    
                    CTTypedNode nodeV;
                    
                    switch (tokenType)
                    {
                        case CTokenType.NUMBER:
                            nodeV = new CTConstValue(lexer.Consume());
                            break;
                        case CTokenType.WORD:

                            if (lexer.MatchsSignature(CTRoot.FUNCTION_CALL_SIG))
                            {
                                var response = CTFunctionCallExpression.Parse(lexer);
                                nodeV = (CTTypedNode)response;
                            }
                            else
                            {
                                nodeV = new CTVariable(lexer.Consume());
                            }
                            break;
                        default:
                            throw new CTLexerException(lexer.Peek(), $"Unexpected token '{lexer.Peek().Span}'.");
                    }

                    valueStack.Push(nodeV);
                    wasOperator = false;
                    break;

                case CTokenType.PLUS:
                case CTokenType.MINUS:
                case CTokenType.BIT_AND:
                case CTokenType.BIT_OR:
                case CTokenType.BIT_NOT:
                case CTokenType.BIT_XOR:
                case CTokenType.BIT_SHIFT_LEFT:
                case CTokenType.BIT_SHIFT_RIGHT:
                case CTokenType.MULTIPLY:
                case CTokenType.DIVIDE:
                case CTokenType.LESS_THAN:
                case CTokenType.MORE_THAN:
                case CTokenType.BOOLEAN_AND:
                case CTokenType.BOOLEAN_OR:
                case CTokenType.BOOLEAN_NOT:
                case CTokenType.EQUALS_EQUALS:
                case CTokenType.LESS_THAN_OR_EQUAL:
                case CTokenType.MORE_THAN_OR_EQUAL:
                {
                    var token = lexer.Consume();

                    if (!wasOperator && token.Options == CTOptions.UNARY)
                        wasOperator = true;

                    if (wasOperator)
                    {
                        valueStack.Push(new CTNodeEmpty());
                        token.Options = CTOptions.UNARY;
                    }

                    while (operatorStack.Count > 0 && !wasOperator &&
                           TokenPriority(operatorStack.Peek()) > TokenPriority(token))
                    {
                        if (!EmptyStack(operatorStack, valueStack))
                            return default;
                    }

                    operatorStack.Push(token);
                    wasOperator = true;
                    break;
                }
                case CTokenType.LEFT_PARENTHESES:
                {
                    var token = lexer.Consume();

                    parenthesisCount++;
                    operatorStack.Push(token);
                    wasOperator = true;
                    break;
                }
                case CTokenType.RIGHT_PARENTHESES:
                {
                    var token = lexer.Consume();

                    parenthesisCount--;
                    if (wasOperator)
                    {
                        valueStack.Push(new CTNodeEmpty());
                        token.Options = CTOptions.UNARY;
                    }

                    while (operatorStack.Count > 0 && operatorStack.Peek().Type != CTokenType.LEFT_PARENTHESES)
                    {
                        if (!EmptyStack(operatorStack, valueStack))
                            return default;
                    }

                    if (operatorStack.Count > 0)
                        operatorStack.Pop(); // Remove the '('
                    else
                    {
                        throw new CTLexerException(token, "Missing '(' for the ')'.");
                    }

                    wasOperator = false;
                    break;
                }
                default: throw new CTLexerException(operatorStack.Peek(), $"Unexpected token '{operatorStack.Peek().Span}' in {hint}.");
            }
        }
        
        while (operatorStack.Count > 0)
            if (!EmptyStack(operatorStack, valueStack))
                return default;
        
        switch (valueStack.Count)
        {
            case 0: throw new CTLexerException(lexer.PeekPrevious(), "Invalid expression");
            case > 1:
            {
                var a = DebugLogNode(valueStack.Pop());
                var b = DebugLogNode(valueStack.Pop());

                throw new CTLexerException(lexer.PeekPrevious(), 
                    $"Missing operator <color=white>{b}</color> <b><color=cyan>?</color></b> <color=white>{a}</color>");
            }
        }

        var tree = valueStack.Pop();
        var node = new CTExpression(tree);
        
        return node;
    }

    private static bool EmptyStack(Stack<CToken> operatorStack, Stack<CTTypedNode> valueStack)
    {
        var op = operatorStack.Pop();

        if (valueStack.Count <= 1)
        {
            throw new CTLexerException(op,
                $"Missing left & right hand values for the operator '{op.Span.Content}'.");
        }
        
        var right = valueStack.Pop();
        var left = valueStack.Pop();

        switch (right)
        {
            case CTNodeEmpty when left is CTNodeEmpty:
                throw new CTLexerException(op,
                    $"Missing left & right hand values for the operator '{op.Span.Content}'.");
            case CTNodeEmpty:
                throw new CTLexerException(op,
                    $"Missing left hand value for the operator '{op.Span.Content}'.");
        }

        var operatorNode = new CTOperator(op, left, right);

        valueStack.Push(operatorNode);
        return true;
    }

    private static string DebugLogNode(CTNode node, bool inMultiplicationChain = false, int parentPriority = -1)
    {
        string str = string.Empty;

        switch (node)
        {
            case CTOperator op:
                int currentPriority = TokenPriority(op.Operator);

                // Check if the current operation is part of a multiplication chain
                bool isMultiplication = op.Operator.Type is CTokenType.MULTIPLY or CTokenType.DIVIDE;
                
                bool shouldAddParentheses = parentPriority > currentPriority;

                // Add parenthesis if entering a new chain of multiplications/divisions
                if (isMultiplication && !inMultiplicationChain || shouldAddParentheses)
                {
                    str += "(";
                }

                // Decide whether to add spaces around the operator
                bool shouldAddSpaces = !(op.Left is CTNodeEmpty || op.Right is CTNodeEmpty);
                string spaces = shouldAddSpaces ? " " : "";

                // Parse left and right operands
                str += DebugLogNode(op.Left, isMultiplication, currentPriority);
                str += spaces + op.Operator.Span.Content + spaces;
                str += DebugLogNode(op.Right, isMultiplication, currentPriority);

                // Close parenthesis if exiting a chain of multiplications/divisions
                if (isMultiplication && !inMultiplicationChain || shouldAddParentheses)
                {
                    str += ")";
                }
            
                break;

            case CTConstValue value:
                str += value.Token.Span.Content;
                break;
            // Include other cases like CTNodeFunctionCall, CTNodeEmpty, etc.
        }

        return str;
    }

    public void SetTypeHint(string spanContent)
    {
        TypeName = spanContent;
    }
}
