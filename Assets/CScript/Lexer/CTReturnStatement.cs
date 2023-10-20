namespace Riten.CScript.Lexer
{
    public class CTReturnStatement : CTStatement
    {
        public readonly CToken ReturnToken;
        public readonly CTExpression ReturnExpression;

        private CTReturnStatement(CToken returnToken, CTExpression returnExpression)
        {
            ReturnToken = returnToken;
            ReturnExpression = returnExpression;
            
            if (returnExpression != null)
                AddChild(returnExpression);
        }
        
        public static CTNode Parse(CTLexer lexer)
        {
            var returnToken = lexer.Consume(CTokenType.RETURN, "Expected 'return' keyword");
            var expression = CTExpression.Parse(lexer, "return expression");
            
            return new CTReturnStatement(returnToken, expression as CTExpression);
        }
    }
}