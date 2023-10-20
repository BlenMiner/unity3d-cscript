using System.Collections.Generic;
using JetBrains.Annotations;

namespace Riten.CScript.Lexer
{
    public class CTFieldDeclaration : CTNode
    {
        public CToken Type;
        public CToken Identifier;
        
        [CanBeNull] public CTExpression Expression;
        
        public CTFieldDeclaration(CToken type, CToken identifier, CTExpression value)
        {
            Type = type;
            Identifier = identifier;
            Expression = value;
        }

        public static CTNode Parse(CTLexer lexer)
        {
            var type = lexer.Consume(CTokenType.WORD, "Expected type for field declaration");
            var identifier = lexer.Consume(CTokenType.WORD, "Expected identifier for field declaration");
            
            if (lexer.Peek().Type == CTokenType.EQUALS)
            {
                lexer.Consume(CTokenType.EQUALS, "Expected '=' in field declaration");
                
                var expression = CTExpression.Parse(lexer, "field declaration");
                
                return new CTFieldDeclaration(
                    type,
                    identifier,
                    (CTExpression)expression
                );
            }
            
            return new CTFieldDeclaration(
                type,
                identifier,
                null
            );
        }
    }
    
    public class CTStructDefinition : CTDefinition
    {
        public CToken Identifier;
        public CTFieldDeclaration[] Fields;
        public CTFunction[] Functions;

        private CTStructDefinition(CToken identifier, CTFieldDeclaration[] fields, CTFunction[] functions)
        {
            Identifier = identifier;
            Fields = fields;
            Functions = functions;
        }

        public static CTNode Parse(CTLexer lexer)
        {
            lexer.Consume(CTokenType.STRUCT, "Expected struct keyword");
            
            var identifier = lexer.Consume(CTokenType.STRUCT, "Expected struct identifier");
            
            lexer.Consume(CTokenType.LEFT_BRACE, "Expected '{' after struct signature");
            
            var fields = new List<CTFieldDeclaration>();
            var functions = new List<CTFunction>();
            
            while (true)
            {
                var token = lexer.Peek();

                if (token.Type == CTokenType.RIGHT_BRACE) break;

                if (lexer.MatchsSignature(CTRoot.FUNCTION_SIG))
                {
                    var response = CTFunction.Parse(lexer);
                    functions.Add((CTFunction)response);
                }
                else if (lexer.MatchsSignature(CTRoot.FIELD_SIG))
                {
                    var response = CTFieldDeclaration.Parse(lexer);
                    fields.Add((CTFieldDeclaration)response);
                }
                else throw new CTLexerException(token, $"Unexpected token '{token.Span}' in struct definition");
            }
            
            lexer.Consume(CTokenType.RIGHT_BRACE, "Expected '}' to end struct definition");
            
            return new CTStructDefinition(
                identifier,
                fields.ToArray(),
                functions.ToArray()
            );
        }
    }
}