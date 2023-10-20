using System;
using Riten.CScript.Lexer;

public class CTRoot : CTNode
{
    public static readonly CTokenType[] FUNCTION_SIG =
    {
        CTokenType.WORD,
        CTokenType.WORD,
        CTokenType.LEFT_PARENTHESES
    };
    
    public static readonly CTokenType[] FUNCTION_CALL_SIG =
    {
        CTokenType.WORD,
        CTokenType.LEFT_PARENTHESES
    };
    
    public static readonly CTokenType[] FIELD_SIG =
    {
        CTokenType.WORD,
        CTokenType.WORD
    };

    public void Parse(CTLexer lexer)
    {
        Children.Clear();
        
        while (!lexer.IsEndOfFile())
        {
            switch (lexer.Peek().Type)
            {
                case CTokenType.SEMICOLON:
                    lexer.Consume();
                    continue;
                case CTokenType.STRUCT:
                    Add(lexer, CTStructDefinition.Parse);
                    break;
                default:
                {
                    if (lexer.MatchsSignature(FUNCTION_SIG))
                    {
                        Add(lexer, CTFunction.Parse);
                        break;
                    }
                    
                    lexer.RegisterError(new CTError(lexer.Peek(), "Unexpected token in root scope"));
                    lexer.SkipUntilRightBrace();
                    break;
                }
            }
        }
    }

    private void Add(CTLexer lexer, Func<CTLexer, CTNode> parser)
    {
        try
        {
            AddChild(parser(lexer));
        }
        catch (CTLexerException ex)
        {
            lexer.RegisterError(ex.Error);
            lexer.SkipUntilRightBrace();
        }
    }
}
