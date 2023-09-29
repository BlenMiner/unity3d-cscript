using System;
using System.Collections.Generic;
using Riten.CScript.Native;

namespace Riten.CScript.Runtime
{
    public static class CTCompiler
    {
        static readonly List<Instruction> s_instructions = new ();

        static void Compile(CTExpression expression)
        {
            CompileExpression(expression.TreeRoot);
        }
        
        static void CompileExpression(CTNode node)
        {
            switch (node)
            {
                case CTOperator op:

                    bool unary = op.Left is CTNodeEmpty;
                    if (!unary)
                    {
                        CompileExpression(op.Left);
                        CompileExpression(op.Right);
                        CompileOperator(op);
                    }
                    else
                    {
                        CompileUnaryOperator(op, op.Right as CTConstValue);
                    }
                    break;
                case CTConstValue val:
                    var value = GetConstValue(val);
                    s_instructions.Add(new Instruction(Opcodes.PUSH_CONST, value));
                    break;
                default: throw new Exception($"Unexpected node type {node.GetType().Name} in expression.");
            }
        }
        
        static void CompileUnaryOperator(CTOperator op, CTConstValue value)
        {
            switch (op.Operator.Type)
            {
                case CTokenType.PLUS: break;
                case CTokenType.MINUS:
                    s_instructions.Add(new Instruction(Opcodes.PUSH_CONST, -GetConstValue(value)));
                    break;
                
                default: throw new NotImplementedException($"Unary operator {op.Operator.Type} not implemented");
            }
        }
        
        static void CompileOperator(CTOperator op)
        {
            switch (op.Operator.Type)
            {
                case CTokenType.PLUS:
                    s_instructions.Add(new Instruction(Opcodes.ADD));
                    break;
                
                default: throw new NotImplementedException($"Operator {op.Operator.Type} not implemented");
            }
        }
        
        static long GetConstValue(CTConstValue op)
        {
            if (long.TryParse(op.Token.Span.Content, out var val))
            {
                return val;
            }
            
            throw new NotImplementedException($"Invalid value: {op.Token.Span.Content}");
        }
        
        static void Compile(CTNode node)
        {
            switch (node)
            {
                case CTExpression expression: Compile(expression); break;
                default: throw new NotImplementedException($"Instruction {node.GetType().Name} not implemented");
            }
        }
        
        public static Instruction[] Compile(CTRoot root)
        {
            s_instructions.Clear();

            for (var i = 0; i < root.Children.Count; i++)
                Compile(root.Children[i]);

            CTOptimizer.Optimize(s_instructions);
            return s_instructions.ToArray();
        }
    }
}