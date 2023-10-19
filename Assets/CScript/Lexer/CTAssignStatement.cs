namespace Riten.CScript.Lexer
{
    public class CTAssignStatement : CTStatement
    {
        public readonly CToken Identifier;
        public readonly CToken EqualSign;
        public readonly CTExpression Expression;

        private CTAssignStatement(CToken identifier, CToken equal, CTExpression expr)
        {
            Identifier = identifier;
            EqualSign = equal;
            Expression = expr;
        }
        
        public static CTNode Parse(CTLexer lexer)
        {
            var identifier = lexer.Consume(CTokenType.WORD, "Expected identifier for assignment");
            var equalSign = lexer.Consume(CTokenType.EQUALS, "Expected '=' in assignment");
            var expression = CTExpression.Parse(lexer, "assign expression");
            
            return new CTAssignStatement(
                identifier,
                equalSign,
                (CTExpression)expression
            );
        }                        
    }
}