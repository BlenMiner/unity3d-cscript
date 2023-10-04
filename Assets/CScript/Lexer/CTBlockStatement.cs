using System.Collections.Generic;
using UnityEngine;

namespace Riten.CScript.Lexer
{
    public class CTBlockStatement : CTStatement
    {
        public readonly CTStatement[] Statements;

        public CTBlockStatement(CTStatement[] statements) : base(CTNodeType.Block)
        {
            Statements = statements;
        }
        
        public static readonly CTokenType[] DECLARE_STATEMENT_SIG =
        {
            CTokenType.WORD,
            CTokenType.WORD,
            CTokenType.EQUALS
        };
    
        public static readonly CTokenType[] ASSIGN_STATEMENT_SIG =
        {
            CTokenType.WORD,
            CTokenType.EQUALS
        };
        
        public static readonly CTokenType[] SWAP_STATEMENT_SIG =
        {
            CTokenType.WORD,
            CTokenType.SWAP,
            CTokenType.WORD
        };
        
        public static CTNodeResponse Parse(IReadOnlyList<CToken> tokens, int i)
        {
            var statements = new List<CTStatement>();
            
            if (tokens[i].Type != CTokenType.LEFT_BRACE)
                throw new CTLexerException(tokens[i], $"Expected '{{', got '{tokens[i].Span}'.");

            ++i;
            
            while (i < tokens.Count)
            {
                var token = tokens[i];

                if (token.Type == CTokenType.RIGHT_BRACE)
                {
                    ++i;
                    break;
                }

                if (token.Type == CTokenType.SEMICOLON)
                {
                    ++i;
                    continue;
                }

                int start = i;

                i = ParseNode(tokens, i, token, statements);

                if (start == i)
                {
                    Debug.LogError($"Infinite loop detected at token {token.Span}.");
                    break;
                }
            }

            return new CTNodeResponse(new CTBlockStatement(statements.ToArray()), i);
        }

        private static int ParseNode(IReadOnlyList<CToken> tokens, int i, CToken token, ICollection<CTStatement> statements)
        {
            switch (token.Type)
            {
                case CTokenType.RETURN:
                {
                    var statement = CTReturnStatement.Parse(tokens, i);
                    statements.Add(statement.Node as CTStatement);
                    i = statement.Index;
                    break;
                }
                case CTokenType.REPEAT:
                {
                    var statement = CTRepeatBlockStatement.Parse(tokens, i);
                    statements.Add(statement.Node as CTStatement);
                    i = statement.Index;
                    break;
                }
                default:
                {
                    if (CTRoot.MatchSignature(tokens, i, DECLARE_STATEMENT_SIG))
                    {
                        var statement = CTDeclareStatement.Parse(tokens, i);
                        statements.Add(statement.Node as CTStatement);
                        i = statement.Index;
                    }
                    else if (CTRoot.MatchSignature(tokens, i, ASSIGN_STATEMENT_SIG))
                    {
                        var statement = CTAssignStatement.Parse(tokens, i);
                        statements.Add(statement.Node as CTStatement);
                        i = statement.Index;
                    }
                    else if (CTRoot.MatchSignature(tokens, i, SWAP_STATEMENT_SIG))
                    {
                        var statement = CTSwapStatement.Parse(tokens, i);
                        statements.Add(statement.Node as CTStatement);
                        i = statement.Index;
                    }
                    else
                    {
                        throw new CTLexerException(token, $"Unexpected token '{token.Span}'. Expected a statement.");
                    }

                    break;
                }
            }

            return i;
        }
    }
}