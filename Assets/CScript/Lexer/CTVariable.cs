using System.Collections.Generic;

namespace Riten.CScript.Lexer
{
    public class CTVariable : CTTypedNode
    {
        public CToken Identifier;
        
        public CTVariable(CToken identifier)
        {
            Identifier = identifier;
        }

        public static CTNode Parse(CTLexer lexer)
        {
            var varName = lexer.Consume(CTokenType.WORD, "Expected variable name.");
            return new CTVariable(varName);
        }
    }
}