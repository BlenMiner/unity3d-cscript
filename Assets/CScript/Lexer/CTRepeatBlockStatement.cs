namespace Riten.CScript.Lexer
{
    public class CTRepeatBlockStatement : CTStatement
    {
        public readonly CToken RepeatToken;
        public readonly CTBlockStatement BlockStatement;
        public readonly CTExpression Count;

        private CTRepeatBlockStatement(CToken repeatToken, CTExpression count, CTBlockStatement blockStatement)
        {
            RepeatToken = repeatToken;
            Count = count;
            BlockStatement = blockStatement;
        }

        public static CTNode Parse(CTLexer lexer)
        {
            var repeat = lexer.Consume(CTokenType.REPEAT, "Expected 'repeat' keyword");
            lexer.Consume(CTokenType.LEFT_PARENTHESES, "Expected '(' after 'repeat' keyword");
            
            var count = CTExpression.Parse(lexer, "repeat count expression");
            
            lexer.Consume(CTokenType.RIGHT_PARENTHESES, "Expected ')' after repeat count expression");
            
            var block = CTBlockStatement.Parse(lexer);
            
            return new CTRepeatBlockStatement(repeat, count as CTExpression, block as CTBlockStatement);
        }
    }
}