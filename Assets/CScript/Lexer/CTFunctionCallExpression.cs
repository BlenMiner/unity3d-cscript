using System.Collections.Generic;

namespace Riten.CScript.Lexer
{
    public class CTFunctionCallExpression : CTTypedNode
    {
        public readonly CToken Identifier;
        public readonly CTFunctionCallArgs Arguments;

        private CTFunctionCallExpression(CToken identifier, CTFunctionCallArgs arguments)
        {
            Identifier = identifier;
            Arguments = arguments;
            
            AddChild(arguments);
        }
        
        public static CTNode Parse(CTLexer lexer)
        {
            var identifier = lexer.Consume(CTokenType.WORD, "Expected function name");
            lexer.Consume(CTokenType.LEFT_PARENTHESES, "Expected '(' after function name");
            
            var args = CTFunctionCallArgs.Parse(lexer);
            
            lexer.Consume(CTokenType.RIGHT_PARENTHESES, "Expected closing parenthesis after function call");
            
            return new CTFunctionCallExpression(identifier, (CTFunctionCallArgs)args);
        }
    }
    
    public class CTFunctionCallArgs : CTNode
    {
        public readonly CTExpression[] Values;

        public CTFunctionCallArgs(CTExpression[] Args)
        {
            Values = Args;
        }
        
        public static CTNode Parse(CTLexer lexer)
        {
            var args = new List<CTExpression>();
            
            bool expectingComma = false;

            while (true)
            {
                var token = lexer.Peek();
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
                        lexer.Consume();
                        continue;

                    default:
                        if (expectingComma)
                            throw new CTLexerException(token, $"Expected comma between arguments, got {token.Type} at {token.Span.Start}.");
                        
                        var node = CTExpression.Parse(lexer, "function call argument");
                        args.Add((CTExpression)node);
                        expectingComma = true;
                        break;
                }
                
                if (cancel) break;
            }

            return new CTFunctionCallArgs(args.ToArray());
        }
    }
}