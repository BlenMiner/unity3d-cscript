using Riten.CScript.Lexer;
using Riten.CScript.Native;

namespace Riten.CScript.Compiler
{
    public class CompiledDeclareStatement : CompiledNode
    {
        public readonly Scope Scope;
        public readonly CompiledExpression Expression;
        
        public CompiledDeclareStatement(CTCompiler compiler, Scope scope, CTDeclareStatement statement, int level) : base(compiler)
        {
            Scope = scope;
            
            statement.Expression.SetTypeHint(statement.Type.Span.Content);
            
            Expression = (CompiledExpression)compiler.CompileNode(scope, statement.Expression, level);

            if (Expression.ExpressionNode.TypeName != statement.Type.Span.Content)
            {
                throw new CTLexerException(statement.Type, 
                    $"Assigning value of type {Expression.ExpressionNode.TypeName} but declared as {statement.Type.Span.Content}.");
            }

            var variableIdentifier = statement.Identifier.Span.Content;
            
            var variable = scope.ReadVariable(variableIdentifier, level);
            scope.RegisterWrite(variableIdentifier);
            
            Compiler.Instructions.Add(new Instruction(Opcodes.POP_TO_SPTR, variable.StackPointer));
        }
    }
}