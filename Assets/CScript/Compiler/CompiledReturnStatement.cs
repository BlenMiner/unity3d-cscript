using System;
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
            
            Expression = (CompiledExpression)compiler.CompileNode(scope, node.ReturnExpression, level);
            
            Compiler.Instructions.Add(new Instruction(Opcodes.POP_TO_SPTR, (long)0));

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
    }
}