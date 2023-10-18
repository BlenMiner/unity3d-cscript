using Riten.CScript.Lexer;
using Riten.CScript.Native;

namespace Riten.CScript.Compiler
{
    public class CompiledReturnStatement : CompiledNode
    {
        public readonly Scope Scope;
        public readonly CompiledExpression Expression;
        
        public CompiledReturnStatement(CTCompiler compiler, Scope scope, CTReturnStatement node, int level) : base(compiler)
        {
            Scope = scope;

            if (scope.ScopeCreator is not CTFunction function)
                throw new CTLexerException(node.ReturnToken, "Return statement isn't inside a function.");
            
            node.ReturnExpression.SetTypeHint(function.TypeName);
            
            if (node.ReturnExpression != null)
                Expression = (CompiledExpression)compiler.CompileNode(scope, node.ReturnExpression, level);

            if (node.ReturnExpression == null)
            {
                if (function.TypeName != null)
                    throw new CTLexerException(node.ReturnToken, "Return type should be void.");
            }
            else if (node.ReturnExpression.TypeName != function.TypeName)
            {
                throw new CTLexerException(node.ReturnToken, 
                    $"Return statement is of type {node.ReturnExpression.TypeName} but function expects {function.TypeName}.");
            }
            
            if (Scope.LocalVariables.Count > 0)
                Compiler.Instructions.Add(new Instruction(Opcodes.POP_TO_SPTR, 0));

            if (Expression != null)
            {
                if (Expression.StackSize >= scope.StackSize)
                {
                    Compiler.Instructions.Add(new Instruction(Opcodes.RETURN));
                }
                else
                {
                    Compiler.Instructions.Add(new Instruction(Opcodes.DISCARD, scope.StackSize - Expression.StackSize));
                    Compiler.Instructions.Add(new Instruction(Opcodes.RETURN));
                }
            }
            else
            {
                Compiler.Instructions.Add(new Instruction(Opcodes.RETURN));
            }
        }
    }
}