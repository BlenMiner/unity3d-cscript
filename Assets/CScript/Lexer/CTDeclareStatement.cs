namespace Riten.CScript.Lexer
{
    public class CTDeclareStatement : CTStatement
    {
        public readonly CToken Type;
        public readonly CToken Identifier;
        public readonly CToken EqualSign;
        public readonly CTExpression Expression;

        private CTDeclareStatement(CToken type, CToken identifier, CToken equal, CTExpression expr)
        {
            Type = type;
            Identifier = identifier;
            EqualSign = equal;
            Expression = expr;
        }
        
        public static CTNode Parse(CTLexer lexer)
        {
            var type = lexer.Consume(CTokenType.WORD, "Expected type for declaration");
            var identifier = lexer.Consume(CTokenType.WORD, "Expected identifier for declaration");
            var equalSign = lexer.Consume(CTokenType.EQUALS, "Expected '=' in declaration");
            
            var expression = CTExpression.Parse(lexer, "declare expression");
            
            return new CTDeclareStatement(
                type,
                identifier,
                equalSign,
                (CTExpression)expression
            );
        }
    }
}