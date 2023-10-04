using Riten.CScript.Lexer;
using Riten.CScript.Native;

namespace Riten.CScript.Compiler
{
    public class CompiledSwapStatement : CompiledNode
    {
        public CompiledSwapStatement(CTCompiler compiler, Scope scope, CTSwapStatement statement, int level) : base(compiler)
        {
            var leftVar = statement.Left.Identifier.Span.Content;
            var rightVar = statement.Right.Identifier.Span.Content;
            
            var leftVarInfo = scope.ReadVariable(leftVar, level);
            var rightVarInfo = scope.ReadVariable(rightVar, level);
            
            Compiler.Instructions.Add(new Instruction(Opcodes.SWAP_SPTR_SPTR, 
                leftVarInfo.StackPointer, rightVarInfo.StackPointer));
        }
    }
}