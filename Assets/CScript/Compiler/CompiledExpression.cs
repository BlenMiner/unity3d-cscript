using System;
using Riten.CScript.Lexer;
using Riten.CScript.Native;

namespace Riten.CScript.Compiler
{
    public class CompiledExpression : CompiledNode
    {
        public readonly CTExpression ExpressionNode;
        public readonly Scope Scope;
        public readonly long StackSize;
        
        public CompiledExpression(CTCompiler compiler, Scope scope, CTExpression expressionNode, int level)
            :base(compiler)
        {
            ExpressionNode = expressionNode;
            Scope = scope;
            
            CompileExpression(ExpressionNode.TreeRoot, level);

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
        
        void CompileExpression(CTNode node, int level)
        {
            switch (node)
            {
                case CTOperator op:

                    bool unary = op.Left is CTNodeEmpty;
                    if (!unary)
                    {
                        CompileExpression(op.Left, level);
                        CompileExpression(op.Right, level);
                        CompileOperator(op);
                    }
                    else
                    {
                        CompileUnaryOperator(op, op.Right as CTConstValue);
                    }
                    break;
                case CTFunctionCallExpression val:
                    var fnName = val.Identifier.Span.Content;
                    // var function = Scope.GetFunction(val.Identifier.Span.Content);
                    //int expectedArgCount = function.Function.Arguments.Values.Count;
                    int actualArgCount = val.Arguments.Values.Length;
                    
                    for (int i = 0; i < actualArgCount; i++)
                        _ = new CompiledExpression(Compiler, Scope, val.Arguments.Values[i], level);
                    
                    Compiler.TemporaryFunctionCalls.Add(Compiler.Instructions.Count, new TempFunctionCall(fnName, actualArgCount, Scope));
                    Compiler.Instructions.Add(new Instruction(Opcodes.CALL, -1, actualArgCount));
                    
                    break;
                case CTConstValue val:
                    var value = GetConstValue(val);
                    Compiler.Instructions.Add(new Instruction(Opcodes.PUSH_CONST, value));
                    break;
                case CTVariable var:
                    var variable = Scope.ReadVariable(var.Identifier.Span.Content, level);
                    Compiler.Instructions.Add(new Instruction(Opcodes.PUSH_FROM_SPTR, variable.StackPointer));
                    break;
                default: throw new Exception($"Unexpected node type {node.GetType().Name} in expression during compilation.");
            }
        }
    }
}