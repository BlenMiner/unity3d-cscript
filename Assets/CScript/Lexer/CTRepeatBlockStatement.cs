using System.Collections.Generic;

namespace Riten.CScript.Lexer
{
    public class CTRepeatBlockStatement : CTStatement
    {
        public readonly CToken RepeatToken;
        public readonly CTBlockStatement BlockStatement;
        public readonly CTExpression Count;
        
        public CTRepeatBlockStatement(CToken repeatToken, CTExpression count, CTBlockStatement blockStatement) : base(CTNodeType.RepeatStatement)
        {
            RepeatToken = repeatToken;
            Count = count;
            BlockStatement = blockStatement;
        }
        
        public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
        {
            if (tokens[i].Type != CTokenType.REPEAT)
                throw new CTLexerException(tokens[i], $"Expected 'repeat', got '{tokens[i].Span}'.");

            ++i;
            
            if (tokens[i].Type != CTokenType.LEFT_PARENTHESES)
                throw new CTLexerException(tokens[i], $"Expected '(', got '{tokens[i].Span}'.");
            
            ++i;

            var count = CTExpression.Parse(tokens, i, "repeat count");
            i = count.Index + 1;

            if (tokens[i].Type != CTokenType.LEFT_BRACE)
                throw new CTLexerException(tokens[i], $"Expected '{{', got '{tokens[i].Span}'. In Repeat statement.");

            var block = CTBlockStatement.Parse(tokens, i);
            i = block.Index;

            return new CTNodeResponse(new CTRepeatBlockStatement(tokens[i - 1], count.Node as CTExpression, block.Node as CTBlockStatement), i);
        }
    }
}