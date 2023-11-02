using Riten.CScript.Lexer;
using Riten.CScript.Native;

namespace Riten.CScript.Compiler
{
    public class CompiledAssignStatement : CompiledNode
    {
        public readonly Scope Scope;
        public readonly CompiledExpression Expression;
        
        public CompiledAssignStatement(CTCompiler compiler, Scope scope, CTAssignStatement statement, int level) : base(compiler)
        {
            Scope = scope;

            var variableIdentifier = statement.Identifier.Span.Content;
            
            var variable = scope.ReadVariable(variableIdentifier, level);
            scope.RegisterWrite(variableIdentifier);
            
            statement.Expression.SetTypeHint(variable.TypeName);
            Expression = (CompiledExpression)compiler.CompileNode(scope, statement.Expression, level);
            
            if (Expression.TypeName != variable.TypeName)
            {
                throw new CTLexerException(statement.Identifier, 
                    $"Assigning value of type {Expression.TypeName} but declared as {variable.TypeName}.");
            }
            
            CTypeResolver.CompilePopToSPTR(Compiler, Expression.TypeName, variable.StackPointer);
        }
    }
}