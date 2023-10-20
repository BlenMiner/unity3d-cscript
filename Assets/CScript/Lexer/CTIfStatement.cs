using System.Collections.Generic;

namespace Riten.CScript.Lexer
{
    public class CTIfStatement : CTStatement
    {
        public struct IfBranch
        {
            public readonly CTExpression Condition;
            public readonly CTBlockStatement BlockStatement;
            
            public IfBranch(CTExpression condition, CTBlockStatement blockStatement)
            {
                Condition = condition;
                BlockStatement = blockStatement;
            }
        }
        
        public readonly IfBranch[] Branches;
        public readonly CTBlockStatement ElseBlockStatement;

        private CTIfStatement(IfBranch[] branches, CTBlockStatement elseBlock)
        {
            Branches = branches;
            ElseBlockStatement = elseBlock;
            
            for (var i = 0; i < branches.Length; i++)
            {
                var branch = branches[i];
                AddChild(branch.Condition);
                AddChild(branch.BlockStatement);
            }
        }
        
        public static CTNode Parse(CTLexer lexer)
        {
            lexer.Consume(CTokenType.IF, "Expected if statement");
            
            var branches = new List<IfBranch>();
            CTBlockStatement elseBlock = null;
            
            while (true)
            {
                lexer.Consume(CTokenType.LEFT_PARENTHESES, "Expected '(' after the 'if' keyword");
                
                var condition = CTExpression.Parse(lexer, "if condition");
                
                lexer.Consume(CTokenType.RIGHT_PARENTHESES, "Expected ')' to close the condition");
                
                var block = CTBlockStatement.Parse(lexer);

                branches.Add(new IfBranch(condition as CTExpression, block as CTBlockStatement));
                
                if (lexer.Peek().Type != CTokenType.ELSE)
                    break;

                lexer.Consume();
                
                if (lexer.Peek().Type != CTokenType.IF)
                {
                    var elseBlockNode = CTBlockStatement.Parse(lexer);
                    elseBlock = elseBlockNode as CTBlockStatement;
                    break;
                }
                
                lexer.Consume();
            }

            return new CTIfStatement(branches.ToArray(), elseBlock);
        }
    }
}