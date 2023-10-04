using System.Collections.Generic;

namespace Riten.CScript.Lexer
{
    public class CTDeclareStatement : CTStatement
    {
        public readonly CTType Type;
        public readonly CToken Identifier;
        public readonly CToken EqualSign;
        public readonly CTExpression Expression;
        
        public CTDeclareStatement(CTType type, CToken identifier, CToken equal, CTExpression expr) 
            : base(CTNodeType.DeclareStatement)
        {
            Type = type;
            Identifier = identifier;
            EqualSign = equal;
            Expression = expr;
        }
        
        public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
        {
            var type = CTType.Parse(tokens, i);
            
            i = type.Index;
            
            var identifier = tokens[i++];
            
            if (identifier.Type != CTokenType.WORD)
                throw new CTLexerException(identifier, $"Expected identifier, got '{identifier.Span}'.");
            
            if (i >= tokens.Count) 
                throw new CTLexerException(identifier, "Expected '=', got end of file.");
            
            var equalSign = tokens[i++];
            
            if (equalSign.Type != CTokenType.EQUALS)
                throw new CTLexerException(equalSign, $"Expected '=', got '{equalSign.Span}'.");
            
            if (i >= tokens.Count)
                throw new CTLexerException(equalSign, "Expected expression, got end of file.");
            
            var expression = CTExpression.Parse(tokens, i);
            
            return new CTNodeResponse(new CTDeclareStatement(
                (CTType)type.Node,
                identifier,
                equalSign,
                (CTExpression)expression.Node
            ), expression.Index);
        }
    }
}