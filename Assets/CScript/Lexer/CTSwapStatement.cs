using System.Collections.Generic;

namespace Riten.CScript.Lexer
{
    public class CTSwapStatement : CTStatement
    {
        public readonly CTVariable Left;
        public readonly CTVariable Right;

        public CTSwapStatement(CTVariable left, CTVariable right) : base(CTNodeType.SWAPStatement)
        {
            Left = left;
            Right = right;
        }
        
        public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
        {
            if (tokens[i].Type != CTokenType.WORD)
                throw new CTLexerException(tokens[i], $"Expected variable name, got {tokens[i]}");
            
            var left = new CTVariable(tokens[i]);
            
            i++;
            
            if (tokens[i].Type != CTokenType.SWAP)
                throw new CTLexerException(tokens[i], $"Expected 'swap', got {tokens[i]}");
            
            i++;
            
            if (tokens[i].Type != CTokenType.WORD)
                throw new CTLexerException(tokens[i], $"Expected variable name, got {tokens[i]}");
            
            var right = new CTVariable(tokens[i]);
            
            return new CTNodeResponse(new CTSwapStatement(left, right), i + 1);
        }
    }
}