using System.Collections.Generic;

namespace Riten.CScript.Lexer
{
    public class CTIfStatement : CTStatement
    {
        public struct IfBranch
        {
            public CTExpression Condition;
            public CTBlockStatement BlockStatement;
            
            public IfBranch(CTExpression condition, CTBlockStatement blockStatement)
            {
                Condition = condition;
                BlockStatement = blockStatement;
            }
        }
        
        public IfBranch[] Branches;
        public CTBlockStatement ElseBlockStatement;

        public CTIfStatement(IfBranch[] branches, CTBlockStatement elseBlock) : base(CTNodeType.IfStatement)
        {
            Branches = branches;
            ElseBlockStatement = elseBlock;
        }
        
        public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
        {
            i = ConsumeToken(CTokenType.IF, tokens, i, "Expected if statement.");
            
            var branches = new List<IfBranch>();
            CTBlockStatement elseBlock = null;
            
            while (true)
            {
                i = ConsumeToken(CTokenType.LEFT_PARENTHESES, tokens, i, "Expected '(' after the 'if' keyword.");
                
                var condition = CTExpression.Parse(tokens, i, "if condition");
                i = condition.Index;

                i = ConsumeToken(CTokenType.RIGHT_PARENTHESES, tokens, i, "Close the boolean expression with ')'.");
                
                var block = CTBlockStatement.Parse(tokens, i);
                i = block.Index;

                branches.Add(new IfBranch(condition.Node as CTExpression, block.Node as CTBlockStatement));
                
                if (tokens[i].Type != CTokenType.ELSE)
                    break;
                
                ++i;
                
                if (tokens[i].Type != CTokenType.IF)
                {
                    var elseBlockNode = CTBlockStatement.Parse(tokens, i);
                    elseBlock = elseBlockNode.Node as CTBlockStatement;
                    i = elseBlockNode.Index;
                    break;
                }
                
                ++i;
            }

            return new CTNodeResponse(new CTIfStatement(branches.ToArray(), elseBlock), i);
        }
    }
}