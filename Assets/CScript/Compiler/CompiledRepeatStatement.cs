using Riten.CScript.Lexer;
using Riten.CScript.Native;

namespace Riten.CScript.Compiler
{
    public class CompiledRepeatStatement : CompiledNode
    {
        public readonly CompiledExpression Count;
        public readonly CompiledBlock Body;
        public readonly CTRepeatBlockStatement BlockStatement;
        
        public CompiledRepeatStatement(CTCompiler comp, Scope scope, CTRepeatBlockStatement node) 
            : base(comp)
        {
            BlockStatement = node;

            Count = new CompiledExpression(comp, scope, node.Count);
            // Top of the stack is the count
            comp.Instructions.Add(new Instruction(Opcodes.REPEAT));
            
            Body = new CompiledBlock(Compiler, new Scope(comp, scope, true), node.BlockStatement);
            
            comp.Instructions.Add(new Instruction(Opcodes.REPEAT_END));
        }
    }
}