using System.Collections.Generic;

namespace Riten.CScript.Lexer
{
    public class CTReturnStatement : CTStatement
    {
        public readonly CToken ReturnToken;
        public readonly CTExpression ReturnExpression;
        
        public CTReturnStatement(CToken returnToken, CTExpression returnExpression) : base(CTNodeType.ReturnStatement)
        {
            ReturnToken = returnToken;
            ReturnExpression = returnExpression;
        }
        
        public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
        {
            var returnToken = tokens[i++];
            
            if (returnToken.Type != CTokenType.RETURN)
                throw new CTLexerException(returnToken, $"Expected 'return', got '{returnToken.Span}'.");
            
            if (i >= tokens.Count)
                throw new CTLexerException(returnToken, "Expected expression, got end of file.");
            
            var expression = CTExpression.Parse(tokens, i);
            
            return new CTNodeResponse(new CTReturnStatement(
                returnToken,
                (CTExpression)expression.Node
            ), expression.Index);
        }
    }
}