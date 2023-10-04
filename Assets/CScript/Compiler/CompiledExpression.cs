using System;
using Riten.CScript.Lexer;
using Riten.CScript.Native;

namespace Riten.CScript.Compiler
{
    public class CompiledExpression : CompiledNode
    {
        public readonly CTExpression ExpressionNode;
        public readonly Scope Scope;
        public long StackSize;
        
        public CompiledExpression(CTCompiler compiler, Scope scope, CTExpression expressionNode)
            :base(compiler)
        {
            ExpressionNode = expressionNode;
            Scope = scope;
            
            CompileExpression(ExpressionNode.TreeRoot);

            StackSize = 1;
        }
        
        static long GetConstValue(CTConstValue op)
        {
            if (long.TryParse(op.Token.Span.Content, out var val))
            {
                return val;
            }
            
            throw new NotImplementedException($"Invalid value: {op.Token.Span.Content}");
        }
        
        void CompileUnaryOperator(CTOperator op, CTConstValue value)
        {
            switch (op.Operator.Type)
            {
                case CTokenType.PLUS: break;
                case CTokenType.MINUS:
                    Compiler.Instructions.Add(new Instruction(Opcodes.PUSH_CONST, -GetConstValue(value)));
                    break;
                
                default: throw new NotImplementedException($"Unary operator {op.Operator.Type} not implemented");
            }
        }
        
        void CompileOperator(CTOperator op)
        {
            switch (op.Operator.Type)
            {
                case CTokenType.PLUS:
                    Compiler.Instructions.Add(new Instruction(Opcodes.ADD));
                    break;
                
                default: throw new NotImplementedException($"Operator {op.Operator.Type} not implemented");
            }
        }
        
        void CompileExpression(CTNode node)
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
                    Compiler.Instructions.Add(new Instruction(Opcodes.PUSH_CONST, value));
                    break;
                case CTVariable var:
                    var variable = Scope.ReadVariable(var.Identifier.Span.Content);
                    Compiler.Instructions.Add(new Instruction(Opcodes.PUSH_FROM_SPTR, variable.StackPointer));
                    break;
                default: throw new Exception($"Unexpected node type {node.GetType().Name} in expression.");
            }
        }
    }
}