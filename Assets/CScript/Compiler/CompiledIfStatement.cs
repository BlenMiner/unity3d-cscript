using System.Collections.Generic;
using Riten.CScript.Lexer;
using Riten.CScript.Native;

namespace Riten.CScript.Compiler
{
    public class CompiledIfStatement : CompiledNode
    {
        public readonly List<CompiledExpression> CompiledExpressions = new();
        public readonly List<CompiledBlock> CompiledBranches = new();
        
        private List<int> m_jmpToEndOfStatement = new();

        public CompiledIfStatement(CTCompiler compiler, Scope scope, CTIfStatement statement, int level) : base(compiler)
        {
            for (int i = 0; i < statement.Branches.Length; ++i)
            {
                var branch = statement.Branches[i];
                
                CompiledExpressions.Add(new CompiledExpression(compiler, scope, branch.Condition, level));
                
                int conditionJMP = compiler.Instructions.Count;
                
                compiler.Instructions.Add(new Instruction(Opcodes.JMP_IF_ZERO, -1));
                
                CompiledBranches.Add(new CompiledBlock(compiler, scope, branch.BlockStatement, level + 1));

                m_jmpToEndOfStatement.Add(compiler.Instructions.Count);
                compiler.Instructions.Add(new Instruction(Opcodes.JMP, -1));
                
                compiler.Instructions[conditionJMP] = new Instruction(Opcodes.JMP_IF_ZERO, compiler.Instructions.Count);
            }
            
            if (statement.ElseBlockStatement != null)
            {
                CompiledBranches.Add(new CompiledBlock(compiler, scope, statement.ElseBlockStatement, level + 1));
            }
            
            int endOfStatement = compiler.Instructions.Count;
            
            for (int i = 0; i < m_jmpToEndOfStatement.Count; ++i)
                compiler.Instructions[m_jmpToEndOfStatement[i]] = new Instruction(Opcodes.JMP, endOfStatement);
        }
    }
}