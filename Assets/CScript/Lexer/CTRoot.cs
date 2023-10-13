using System;
using System.Collections.Generic;
using Riten.CScript.Lexer;

public class CTRoot : CTNode
{
    readonly List<CTNode> m_children = new ();
    
    public IReadOnlyList<CTNode> Children => m_children;

    private readonly Action<CTLexerException> Error;

    public CTRoot(Action<CTLexerException> Error) : base(CTNodeType.Root)
    {
        this.Error = Error;
    }
    
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

    public void Parse(IReadOnlyList<CToken> tokens)
    {
        m_children.Clear();
        
        int index = 0;

        while (index < tokens.Count)
        {
            switch (tokens[index].Type)
            {
                case CTokenType.SEMICOLON:
                    index++;
                    continue;
                case CTokenType.STRUCT:
                    Add(tokens, CTStructDefinition.Parse, ref index);
                    break;
                default:
                {
                    if (MatchSignature(tokens, index, FUNCTION_SIG))
                    {
                        Add(tokens, CTFunction.Parse, ref index);
                    }
                    else
                    {
                
                        Error(new CTLexerException(tokens[index], $"Unexpected token '{tokens[index].Span}' in root scope."));
                        index = SkipUntilRightBrace(tokens, index);
                    }

                    break;
                }
            }
        }
    }

    private void Add(IReadOnlyList<CToken> tokens, Func<IReadOnlyList<CToken>, int, CTNodeResponse> parser, ref int index)
    {
        try
        {
            var response = parser(tokens, index);
            m_children.Add(response.Node);
            index = response.Index;
        }
        catch (CTLexerException ex)
        {
            Error(ex);
            index = SkipUntilRightBrace(tokens, index);
        }
    }
    
    public static bool MatchSignature(IReadOnlyList<CToken> tokens, int index, IReadOnlyList<CTokenType> types)
    {
        if (index >= tokens.Count)
            return false;

        if (tokens.Count - index < types.Count)
            return false;

        for (var i = 0; i < types.Count; i++)
        {
            var type = types[i];
            
            if (type == CTokenType.NULL) continue;
            
            if (tokens[index + i].Type != type)
                return false;
        }

        return true;
    }

    private static int SkipUntilRightBrace(IReadOnlyList<CToken> tokens, int index)
    {
        while (index < tokens.Count && tokens[index].Type != CTokenType.RIGHT_BRACE)
            index++;

        index++;

        return index;
    }
}
