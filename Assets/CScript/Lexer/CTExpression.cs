using System.Collections.Generic;
using UnityEngine;

public struct CTNodeResponse
{
    public readonly CTNode Node;
    public readonly int Index;
    
    public CTNodeResponse(CTNode node, int index)
    {
        Node = node;
        Index = index;
    }
}

[System.Serializable]
public class CTExpression : CTNode
{
    public readonly CTNode TreeRoot;

    public CTExpression(CTNode tree) : base(CTNodeType.Expression)
    {
        TreeRoot = tree;
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
    
    public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
    {
        var valueStack = new Stack<CTNode>();
        var operatorStack = new Stack<CToken>();

        valueStack.Clear();
        operatorStack.Clear();
        
        bool wasOperator = true;
        
        while (i < tokens.Count)
        {
            var token = tokens[i];
            
            if (token.Type == CTokenType.SEMICOLON)
            {
                i++;
                break;
            }

            switch (token.Type)
            {
                case CTokenType.NUMBER:
                case CTokenType.WORD:
                    if (wasOperator == false)
                        throw new CTLexerException(token, $"Missing operator for '{token.Span}'.");
                    
                    valueStack.Push(new CTConstValue(token));
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

                    if (!wasOperator && token.Options == CTOptions.UNARY)
                        wasOperator = true;
                    
                    if (wasOperator)
                    {
                        valueStack.Push(new CTNodeEmpty());
                        token.Options = CTOptions.UNARY;
                    }

                    while (operatorStack.Count > 0 && !wasOperator && TokenPriority(operatorStack.Peek()) > TokenPriority(token))
                    {
                        if (!EmptyStack(operatorStack, valueStack))
                            return default;
                    }

                    operatorStack.Push(token);
                    wasOperator = true;
                    break;
                
                case CTokenType.LEFT_PARENTHESES:
                    operatorStack.Push(token);
                    wasOperator = true;
                    break;
                
                case CTokenType.RIGHT_PARENTHESES:
                    
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
                        operatorStack.Pop();  // Remove the '('
                    else
                    {
                        throw new CTLexerException(token, "Missing '(' for the ')'.");
                    }
                    
                    wasOperator = false;
                    break;
                default: throw new CTLexerException(token, $"Unexpected token '{token.Span}' in expression.");
            }
            
            i++;
        }
        
        while (operatorStack.Count > 0)
            if (!EmptyStack(operatorStack, valueStack))
                return default;
        
        switch (valueStack.Count)
        {
            case 0: throw new CTLexerException(tokens[i - 1], "Invalid expression");
            case > 1:
            {
                var a = DebugLogNode(valueStack.Pop());
                var b = DebugLogNode(valueStack.Pop());

                throw new CTLexerException(tokens[i - 1], 
                    $"Missing operator <color=white>{b}</color> <b><color=cyan>?</color></b> <color=white>{a}</color>");
            }
        }

        var tree = valueStack.Pop();
        var node = new CTExpression(tree);
        
        // Debug.Log(DebugLogNode(tree));
        
        return new CTNodeResponse(node, i);
    }

    private static bool EmptyStack(Stack<CToken> operatorStack, Stack<CTNode> valueStack)
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

}
