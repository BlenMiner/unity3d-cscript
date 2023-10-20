using System.Collections.Generic;
using UnityEngine;

namespace Riten.CScript.Lexer
{
    public class CTBlockStatement : CTStatement
    {
        public readonly CTStatement[] Statements;

        public CTBlockStatement(CTStatement[] statements)
        {
            Statements = statements;

            for (var i = 0; i < statements.Length; i++)
            {
                var statement = statements[i];
                AddChild(statement);
            }
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
        
        public static CTNode Parse(CTLexer lexer)
        {
            var statements = new List<CTStatement>();
            
            lexer.Consume(CTokenType.LEFT_BRACE, "Expected '{' at the start of a block statement");
            
            while (true)
            {
                var token = lexer.Peek();

                if (token.Type == CTokenType.RIGHT_BRACE)
                {
                    lexer.Consume();
                    break;
                }

                if (token.Type == CTokenType.SEMICOLON)
                {
                    lexer.Consume();
                    continue;
                }

                int start = lexer.ParsingIndex;

                ParseNode(lexer, token, statements);

                if (start == lexer.ParsingIndex)
                {
                    Debug.LogError($"Infinite loop detected at token {token.Span}.");
                    break;
                }
            }
            
            return new CTBlockStatement(statements.ToArray());
        }
        
        private static void ParseNode(CTLexer lexer, CToken token, ICollection<CTStatement> statements)
        {
            switch (token.Type)
            {
                case CTokenType.RETURN:
                {
                    var statement = CTReturnStatement.Parse(lexer);
                    statements.Add(statement as CTStatement);
                    break;
                }
                case CTokenType.IF:
                {
                    var statement = CTIfStatement.Parse(lexer);
                    statements.Add(statement as CTStatement);
                    break;
                }
                case CTokenType.REPEAT:
                {
                    var statement = CTRepeatBlockStatement.Parse(lexer);
                    statements.Add(statement as CTStatement);
                    break;
                }
                default:
                {
                    if (lexer.MatchsSignature(DECLARE_STATEMENT_SIG))
                    {
                        var statement = CTDeclareStatement.Parse(lexer);
                        statements.Add(statement as CTStatement);
                    }
                    else if (lexer.MatchsSignature(ASSIGN_STATEMENT_SIG))
                    {
                        var statement = CTAssignStatement.Parse(lexer);
                        statements.Add(statement as CTStatement);
                    }
                    else if (lexer.MatchsSignature(SWAP_STATEMENT_SIG))
                    {
                        var statement = CTSwapStatement.Parse(lexer);
                        statements.Add(statement as CTStatement);
                    }
                    else
                    {
                        throw new CTLexerException(token, $"Unexpected token '{token.Span}'. Expected a statement.");
                    }

                    break;
                }
            }
        }
    }
}