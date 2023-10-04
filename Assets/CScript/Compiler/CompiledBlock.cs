using System.Collections.Generic;
using Riten.CScript.Lexer;

namespace Riten.CScript.Compiler
{
    public class CompiledBlock : CompiledNode
    {
        public readonly CTBlockStatement BlockStatement;
        public readonly List<CompiledNode> CompiledBody = new();

        public CompiledBlock(CTCompiler compiler, Scope scope, CTBlockStatement blockStatement, int level) : base(compiler)
        {
            BlockStatement = blockStatement;
            
            for (var i = 0; i < blockStatement.Statements.Length; i++)
            {
                var node = blockStatement.Statements[i];
                CompiledBody.Add(compiler.CompileNode(scope, node, level));
            }
        }
    }
}