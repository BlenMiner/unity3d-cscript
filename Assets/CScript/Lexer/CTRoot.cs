using System;
using System.Collections.Generic;
using Riten.CScript.Lexer;

[Serializable]
public class CTRoot : CTNode
{
    protected List<CTNode> m_children = new ();
    
    public List<CTLexerException> Errors { get; private set; } = new();
    
    public IReadOnlyList<CTNode> Children => m_children;

    public CTRoot() : base(CTNodeType.Root) {}
    
    public static readonly CTokenType[] FUNCTION_SIG =
    {
        CTokenType.WORD,
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
            if (tokens[index].Type == CTokenType.SEMICOLON)
            {
                index++;
                continue;
            }
            
            if (tokens[index].Type == CTokenType.STRUCT)
            {
                Add(tokens, CTStructDefinition.Parse, ref index);
            }
            else if (MatchSignature(tokens, index, FUNCTION_SIG))
            {
                Add(tokens, CTFunction.Parse, ref index);
            }
            else
            {
                Errors.Add(new CTLexerException(tokens[index], $"Unexpected token '{tokens[index].Span}' in root scope."));
                index = SkipUntilSemicolon(tokens, index);
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
            Errors.Add(ex);
            index = SkipUntilSemicolon(tokens, index);
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

    private static int SkipUntilSemicolon(IReadOnlyList<CToken> tokens, int index)
    {
        while (index < tokens.Count && tokens[index].Type != CTokenType.SEMICOLON)
            index++;

        index++;

        return index;
    }
}
