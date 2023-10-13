using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Riten.CScript.Lexer
{
    public class CTFieldDeclaration : CTNode
    {
        public CTType Type;
        public CToken Identifier;
        
        [CanBeNull] public CTExpression Expression;
        
        public CTFieldDeclaration(CTType type, CToken identifier, CTExpression value) : base(CTNodeType.FieldDeclaration)
        {
            Type = type;
            Identifier = identifier;
            Expression = value;
        }

        public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
        {
            if (i >= tokens.Count)
                throw new CTLexerException(tokens[i], "Expected type, got end of file in field declaration.");
            
            var type = CTType.Parse(tokens, i);
            
            i = type.Index;
            
            if (i >= tokens.Count)
                throw new CTLexerException(tokens[i], "Expected identifier, got end of file in field declaration.");
            
            var identifier = tokens[i++];
            
            if (identifier.Type != CTokenType.WORD)
                throw new CTLexerException(identifier, $"Expected identifier, got '{identifier.Span}' in field declaration.");
            
            if (i >= tokens.Count)
                throw new CTLexerException(tokens[i], "Expected '=', got end of file in field declaration.");
            
            var equals = tokens[i++];

            if (equals.Type != CTokenType.SEMICOLON)
            {
                if (equals.Type != CTokenType.EQUALS)
                    throw new CTLexerException(equals, $"Expected '=', got '{equals.Span}' in field declaration.");

                if (i >= tokens.Count)
                    throw new CTLexerException(tokens[i], "Expected expression, got end of file in field declaration.");
                
                var expression = CTExpression.Parse(tokens, i, "field declaration");
            
                i = expression.Index;
            
                return new CTNodeResponse(new CTFieldDeclaration(
                    (CTType)type.Node,
                    identifier,
                    (CTExpression)expression.Node
                ), i);
            }
            
            return new CTNodeResponse(new CTFieldDeclaration(
                (CTType)type.Node,
                identifier,
                null
            ), i);
        }
    }
    
    public class CTStructDefinition : CTNode
    {
        public CTType Identifier;
        public CTFieldDeclaration[] Fields;
        public CTFunction[] Functions;
        
        public CTStructDefinition(CTType identifier, CTFieldDeclaration[] fields, CTFunction[] functions) : base(CTNodeType.StructDefinition)
        {
            Identifier = identifier;
            Fields = fields;
            Functions = functions;
        }

        public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
        {
            var structKeyword = tokens[i++];
            
            if (structKeyword.Type != CTokenType.STRUCT)
                throw new CTLexerException(structKeyword, $"Expected 'struct', got '{structKeyword.Span}'.");
            
            if (i >= tokens.Count)
                throw new CTLexerException(structKeyword, "Expected identifier, got end of file in struct definition.");
            
            var identifier = CTType.Parse(tokens, i);

            i = identifier.Index;

            var startBrace = tokens[i++];

            if (startBrace.Type != CTokenType.LEFT_BRACE)
                throw new CTLexerException(startBrace, $"Expected '{{', got '{startBrace.Span}' in struct definition.");

            var fields = new List<CTFieldDeclaration>();
            var functions = new List<CTFunction>();

            while (i < tokens.Count)
            {
                var token = tokens[i];

                if (token.Type == CTokenType.RIGHT_BRACE) break;

                if (CTRoot.MatchSignature(tokens, i, CTRoot.FUNCTION_SIG))
                {
                    var response = CTFunction.Parse(tokens, i);
                    functions.Add((CTFunction)response.Node);
                    i = response.Index;
                }
                else if (CTRoot.MatchSignature(tokens, i, CTRoot.FIELD_SIG))
                {
                    var response = CTFieldDeclaration.Parse(tokens, i);
                    fields.Add((CTFieldDeclaration)response.Node);
                    i = response.Index;
                }
                else throw new CTLexerException(token, $"Unexpected token '{token.Span}' in struct definition.");
            }

            var endBrace = tokens[i++];

            if (endBrace.Type != CTokenType.RIGHT_BRACE)
                throw new CTLexerException(endBrace, $"Expected '}}', got '{endBrace.Span}' in struct definition.");

            return new CTNodeResponse(new CTStructDefinition(
                (CTType)identifier.Node,
                fields.ToArray(),
                functions.ToArray()
            ), i);
        }
    }
}