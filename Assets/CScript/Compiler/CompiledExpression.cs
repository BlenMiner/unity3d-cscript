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
            
            ParseExpressionTypes(scope, ExpressionNode.TreeRoot, level);
            
            ExpressionNode.TypeName = ExpressionNode.TreeRoot.TypeName;
            
            CompileExpression(ExpressionNode.TreeRoot, level);

            StackSize = 1;
        }
        
        static long GetConstValue(CTConstValue op)
        {
            var val = CTypeResolver.GetValueBits(op.TypeName, op.ValueString);
            return val;
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
                    CTypeResolver.CompileAdd(Compiler, op.Left.TypeName, op.Right.TypeName);
                    break;
                
                case CTokenType.LESS_THAN_OR_EQUAL:
                    Compiler.Instructions.Add(new Instruction(Opcodes.LESS_OR_EQUAL));
                    break;
                
                default: throw new NotImplementedException($"Operator {op.Operator.Type} not implemented");
            }
        }

        void ParseExpressionTypes(Scope scope, CTTypedNode node, int level)
        {
            switch (node)
            {
                case CTOperator op:

                    ParseExpressionTypes(scope, op.Left, level);
                    ParseExpressionTypes(scope, op.Right, level);
                    
                    var typeLeft = op.Left.TypeName;
                    var typeRight = op.Right.TypeName;
                    
                    if (typeLeft != typeRight)
                        throw new CTLexerException(op.Operator, $"Cannot perform operation on different types: {typeLeft} and {typeRight}.");
                    
                    node.TypeName = typeLeft;
                    
                    break;
                case CTVariable variable:
                    variable.TypeName = scope.ReadVariable(variable.Identifier.Span.Content, level).TypeName;
                    break;
                
                case CTFunctionCallExpression fnCall:
                    fnCall.TypeName = scope.GetFunction(fnCall.Identifier.Span.Content).Function.FunctionType.Span.Content;
                    break;

                case CTConstValue: break;
                
                default: throw new NotImplementedException($"Not implemented: node '{node.GetType().Name}' in expression during compilation type checking.");
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
                    int actualArgCount = val.Arguments.Values.Length;
                    
                    for (int i = 0; i < actualArgCount; i++)
                        _ = new CompiledExpression(Compiler, Scope, val.Arguments.Values[i], level);
                    
                    Compiler.TemporaryFunctionCalls.Add(Compiler.Instructions.Count, new TempFunctionCall(fnName, actualArgCount, Scope));

                    Compiler.Instructions.Add(actualArgCount > 0
                        ? new Instruction(Opcodes.CALL_ARGS, -1, actualArgCount)
                        : new Instruction(Opcodes.CALL, -1, actualArgCount));

                    break;
                case CTConstValue val:
                    var value = GetConstValue(val);
                    Compiler.Instructions.Add(new Instruction(Opcodes.PUSH_CONST, value));
                    break;
                case CTVariable var:
                    var variable = Scope.ReadVariable(var.Identifier.Span.Content, level);
                    Compiler.Instructions.Add(new Instruction(Opcodes.PUSH_SPTR, variable.StackPointer));
                    break;
                default: throw new Exception($"Unexpected node type {node.GetType().Name} in expression during compilation.");
            }
        }
    }
}