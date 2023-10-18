using System.Collections.Generic;

namespace Riten.CScript.Lexer
{
    public class CTFunctionCallExpression : CTTypedNode
    {
        public readonly CToken Identifier;
        public readonly CTFunctionCallArgs Arguments;
        
        public CTFunctionCallExpression(CToken identifier, CTFunctionCallArgs arguments) : base(CTNodeType.FunctionCall)
        {
            Identifier = identifier;
            Arguments = arguments;
        }
        
        public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
        {
            i = ConsumeToken(CTokenType.WORD, out var identifier, tokens, i, "Expected function name.");
            i = ConsumeToken(CTokenType.LEFT_PARENTHESES, tokens, i, "Expected function open parenthesis.");
            
            var args = CTFunctionCallArgs.Parse(tokens, i);
            i = args.Index;
            
            i = ConsumeToken(CTokenType.RIGHT_PARENTHESES, tokens, i, "Expected function closing parenthesis.");
            i--;
            
            return new CTNodeResponse(new CTFunctionCallExpression(identifier, (CTFunctionCallArgs)args.Node), i);
        }
    }
    
    public class CTFunctionCallArgs : CTNode
    {
        public readonly CTExpression[] Values;

        public CTFunctionCallArgs(CTExpression[] Args) : base(CTNodeType.FunctionArgs)
        {
            this.Values = Args;
        }
        
        public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
        {
            var args = new List<CTExpression>();
            
            bool expectingComma = false;

            while (i < tokens.Count)
            {
                var token = tokens[i];
                bool cancel = false;

                switch (token.Type)
                {
                    case CTokenType.RIGHT_PARENTHESES:
                        cancel = true; 
                        break;

                    case CTokenType.COMMA:
                        if (!expectingComma)
                            throw new CTLexerException(token, $"Unexpected comma at {token.Span.Start}.");
                        
                        expectingComma = false;
                        ++i;
                        continue;

                    default:
                        if (expectingComma)
                            throw new CTLexerException(token, $"Expected comma between arguments, got {token.Type} at {token.Span.Start}.");
                        
                        var node = CTExpression.Parse(tokens, i, "function call argument");
                        args.Add((CTExpression)node.Node);
                        i = node.Index;
                        expectingComma = true;
                        break;
                }
                
                if (cancel) break;
            }

            return new CTNodeResponse(new CTFunctionCallArgs(args.ToArray()), i);
        }
    }
}