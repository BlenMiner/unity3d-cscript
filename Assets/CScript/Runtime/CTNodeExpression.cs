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
public class CTNodeExpression : CTNode
{
    public readonly CTNode TreeRoot;

    public CTNodeExpression(CTNode tree) : base(CTNodeType.Expression)
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
    
    public static CTNodeResponse Parse(CScript script, int i)
    {
        var tokens = script.Tokens;
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
                        throw new CScriptException(token, $"Missing operator for '{token.Span}'.");
                    
                    valueStack.Push(new CTNodeValue(token));
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
                        throw new CScriptException(token, "Missing '(' for the ')'.");
                    }
                    
                    wasOperator = false;
                    break;
                default: throw new CScriptException(token, $"Unexpected token '{token.Span}' in expression.");
            }
            
            i++;
        }
        
        while (operatorStack.Count > 0)
            if (!EmptyStack(operatorStack, valueStack))
                return default;
        
        switch (valueStack.Count)
        {
            case 0: throw new CScriptException(tokens[i - 1], "Invalid expression");
            case > 1:
            {
                var a = DebugLogNode(valueStack.Pop());
                var b = DebugLogNode(valueStack.Pop());

                throw new CScriptException(tokens[i - 1], 
                    $"Missing operator <color=white>{b}</color> <b><color=cyan>?</color></b> <color=white>{a}</color>");
            }
        }

        var tree = valueStack.Pop();
        var node = new CTNodeExpression(tree);
        
        Debug.Log(DebugLogNode(tree));
        
        return new CTNodeResponse(node, i);
    }

    private static bool EmptyStack(Stack<CToken> operatorStack, Stack<CTNode> valueStack)
    {
        var op = operatorStack.Pop();

        if (valueStack.Count <= 1)
        {
            throw new CScriptException(op,
                $"Missing left & right hand values for the operator '{op.Span.Content}'.");
        }
        
        var right = valueStack.Pop();
        var left = valueStack.Pop();

        switch (right)
        {
            case CTNodeEmpty when left is CTNodeEmpty:
                throw new CScriptException(op,
                    $"Missing left & right hand values for the operator '{op.Span.Content}'.");
            case CTNodeEmpty:
                throw new CScriptException(op,
                    $"Missing left hand value for the operator '{op.Span.Content}'.");
        }

        var operatorNode = new CTNodeOperator(op, left, right);

        valueStack.Push(operatorNode);
        return true;
    }

    private static string DebugLogNode(CTNode node, int lastPriority = 0)
    {
        string str = string.Empty;
        
        switch (node)
        {
            case CTNodeOperator op:

                int p = TokenPriority(op.Operator);
                bool shouldParenthesis = p == 1 && lastPriority != 1;
                if (shouldParenthesis)
                {
                    str += '(';
                }
                
                str += DebugLogNode(op.Left, p);
                str += (op.Left is CTNodeEmpty ? string.Empty : " ") + op.Operator.Span.Content + (op.Right is CTNodeEmpty || op.Left is CTNodeEmpty ? string.Empty : " ");
                str += DebugLogNode(op.Right, p);
                
                if (shouldParenthesis)
                {
                    str += ')';
                }
                break;
            case CTNodeValue value:
                str += value.Token.Span.Content;
                break;
        }

        return str;
    }
}
