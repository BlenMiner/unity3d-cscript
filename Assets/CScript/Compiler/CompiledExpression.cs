using System;
using Riten.CScript.Lexer;
using Riten.CScript.Native;
using Unity.Plastic.Antlr3.Runtime.Tree;

namespace Riten.CScript.Compiler
{
    public class CompiledExpression : CompiledNode
    {
        public readonly CTExpression ExpressionNode;
        public readonly Scope Scope;
        public readonly long StackSize;
        public string TypeName => ExpressionNode.TypeName;
        
        private readonly string m_typeHint;
        
        public CompiledExpression(CTCompiler compiler, Scope scope, CTExpression expressionNode, int level)
            :base(compiler)
        {
            ExpressionNode = expressionNode;
            Scope = scope;
            
            m_typeHint = expressionNode.TypeName ?? "i64";
            
            ParseExpressionTypes(scope, ExpressionNode.TreeRoot, level);

            expressionNode.TypeName = ExpressionNode.TreeRoot.TypeName;
            
            CompileExpression(ExpressionNode.TreeRoot, level);

            StackSize = CTypeResolver.GetBuiltinSize(ExpressionNode.TypeName);
        }
        
        long GetConstValue(CTConstValue value, bool negateResult)
        {
            var val = CTypeResolver.GetValueBits(value.TypeName, value.ValueString, negateResult);
            return val;
        }
        
        void CompileUnaryOperator(CTOperator op, CTConstValue value)
        {
            switch (op.Operator.Type)
            {
                case CTokenType.PLUS: break;
                case CTokenType.MINUS:
                    CTypeResolver.CompilePush(Compiler, value.TypeName, value.ValueString, true);
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
                    CTypeResolver.CompileLessOrEqual(Compiler, op.Left.TypeName, op.Right.TypeName);
                    break;
                
                case CTokenType.MINUS:
                    CTypeResolver.CompileMinus(Compiler, op.Left.TypeName, op.Right.TypeName);
                    break;
                
                case CTokenType.MULTIPLY:
                    CTypeResolver.CompileMult(Compiler, op.Left.TypeName, op.Right.TypeName);
                    break;
                
                case CTokenType.DIVIDE:
                    CTypeResolver.CompileDiv(Compiler, op.Left.TypeName, op.Right.TypeName);
                    break;
                
                case CTokenType.BIT_AND:
                    CTypeResolver.CompileAnd(Compiler, op.Left.TypeName, op.Right.TypeName);
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

                    switch (typeLeft)
                    {
                        case CTTypedNode.UNSET_TYPE when typeRight == CTTypedNode.UNSET_TYPE:
                            op.Left.TypeName = m_typeHint;
                            op.Right.TypeName = m_typeHint;
                        
                            typeLeft = m_typeHint;
                            typeRight = m_typeHint;
                            break;
                        case CTTypedNode.UNSET_TYPE:
                            op.Left.TypeName = typeRight;
                            typeLeft = typeRight;
                            break;
                        default:
                        {
                            if (typeRight == CTTypedNode.UNSET_TYPE)
                            {
                                op.Right.TypeName = typeLeft;
                                typeRight = typeLeft;
                            }

                            break;
                        }
                    }
                    
                    if (typeLeft != typeRight)
                        throw new CTLexerException(op.Operator, $"Cannot perform operation on different types: {typeLeft} and {typeRight}.");
                    
                    node.TypeName = typeLeft;
                    
                    break;
                case CTVariable variable:
                    variable.TypeName = scope.ReadVariable(variable.Identifier.Span.Content, level).TypeName;
                    break;
                
                case CTFunctionCallExpression fnCall:

                    if (!scope.TryGetFunctionSignature(fnCall.Identifier.Span.Content, out var functionSig))
                        throw new CTLexerException(fnCall.Identifier, $"Function {fnCall.Identifier.Span.Content} not found.");
                    
                    fnCall.TypeName = functionSig.ReturnType;
                    break;

                case CTConstValue constType:
                    if (constType.TypeName == CTTypedNode.UNSET_TYPE)
                        constType.TypeName = m_typeHint;
                    break;
                
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
                    int argSize = 0;

                    if (!Scope.TryGetFunctionSignature(fnName, out var functionSig))
                        throw new CTLexerException((CToken)default, $"Function {fnName} not found.");

                    for (int i = 0; i < actualArgCount; i++)
                    {
                        var expr = val.Arguments.Values[i];
                        expr.SetTypeHint(functionSig.Arguments[i].Type);
                        argSize += CTypeResolver.GetBuiltinSize(expr.TypeName);
                        
                        _ = new CompiledExpression(Compiler, Scope, expr, level);
                    }

                    // ValidateFunctionArgumentTypes(val.Arguments.Values, compiledArgs);
                    
                    Compiler.TemporaryFunctionCalls.Add(Compiler.Instructions.Count, new TempFunctionCall(fnName, actualArgCount, Scope));

                    Compiler.Instructions.Add(actualArgCount > 0
                        ? new Instruction(Opcodes.CALL_ARGS, -1, argSize)
                        : new Instruction(Opcodes.CALL, -1, argSize));

                    break;
                case CTConstValue val:
                    CTypeResolver.CompilePush(Compiler, val.TypeName, val.ValueString, false);
                    break;
                case CTVariable var:
                    var variable = Scope.ReadVariable(var.Identifier.Span.Content, level);
                    CTypeResolver.CompilePushSPTR(Compiler, variable.TypeName, variable.StackPointer);
                    break;
                default: throw new Exception($"Unexpected node type {node.GetType().Name} in expression during compilation.");
            }
        }

        /*private void ValidateFunctionArgumentTypes(CTExpression[] argumentsValues, List<CompiledExpression> compiledArgs)
        {
            for (int i = 0; i < argumentsValues.Length; i++)
            {
                var arg = argumentsValues[i];
                var compiledArg = compiledArgs[i];
                
                if (arg.TypeName != compiledArg.TypeName)
                    throw new CTLexerException(default, $"Argument {i} of function call expects type {arg.TypeName} but got {compiledArg.TypeName}.");
            }
        }*/
    }
}