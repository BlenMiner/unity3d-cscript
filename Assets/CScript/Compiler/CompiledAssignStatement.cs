using Riten.CScript.Lexer;
using Riten.CScript.Native;

namespace Riten.CScript.Compiler
{
    public class CompiledAssignStatement : CompiledNode
    {
        public readonly Scope Scope;
        public readonly CompiledExpression Expression;
        
        public CompiledAssignStatement(CTCompiler compiler, Scope scope, CTAssignStatement statement) : base(compiler)
        {
            Scope = scope;
            Expression = compiler.CompileNode(scope, statement.Expression) as CompiledExpression;

            var variableIdentifier = statement.Identifier.Span.Content;
            
            var variable = scope.ReadVariable(variableIdentifier);
            scope.RegisterWrite(variableIdentifier);
            
            Compiler.Instructions.Add(new Instruction(Opcodes.POP_TO_SPTR, variable.StackPointer));
        }
    }
}