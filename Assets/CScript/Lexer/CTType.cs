using System.Collections.Generic;

namespace Riten.CScript.Lexer
{
    public class CTType : CTNode
    {
        public CToken TypeToken;
        
        public CTType(CToken token) : base(CTNodeType.Type)
        {
            TypeToken = token;
        }
        
        public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
        {
            var type = tokens[i++];
            
            if (type.Type != CTokenType.WORD)
                throw new CTLexerException(type, $"Expected type, got '{type.Span}'.");

            return new CTNodeResponse(new CTType(type), i);
        }
    }
}