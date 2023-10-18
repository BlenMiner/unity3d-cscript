using System.Collections.Generic;

namespace Riten.CScript.Lexer
{
    public class CTVariable : CTTypedNode
    {
        public CToken Identifier;
        
        public CTVariable(CToken identifier) : base(CTNodeType.Variable)
        {
            Identifier = identifier;
        }

        public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
        {
            if (i >= tokens.Count)
                throw new CTLexerException(default, "Unexpected end of file, expected a variable.");
            
            if (tokens[i].Type != CTokenType.WORD)
                throw new CTLexerException(tokens[i], $"Expected variable name, got {tokens[i]}");
            
            return new CTNodeResponse(new CTVariable(tokens[i]), i + 1);
        }
    }
}