using System;
using System.Collections.Generic;

[System.Serializable]
public class CTRoot : CTNode
{
    protected List<CTNode> m_children = new ();
    
    public IReadOnlyList<CTNode> Children => m_children;

    public CTRoot() : base(CTNodeType.Root) {}
    
    public static readonly CTokenType[] FUNCTION_SIG =
    {
        CTokenType.WORD,
        CTokenType.WORD,
        CTokenType.LEFT_PARENTHESES
    };
    
    public void Parse(CScript script)
    {
        m_children.Clear();
        
        int index = 0;

        while (index < script.Tokens.Count)
        {
            if (script.Tokens[index].Type == CTokenType.SEMICOLON)
            {
                index++;
                continue;
            }
            
            if (MatchSignature(script, index, FUNCTION_SIG))
            {
                Add(script, CTFunctionDeclaration.Parse, ref index);
            }
            else
            {
                Add(script, CTExpression.Parse, ref index);
            }
        }
    }

    private void Add(CScript script, Func<CScript, int, CTNodeResponse> parser, ref int index)
    {
        try
        {
            var response = parser(script, index);
            m_children.Add(response.Node);
            index = response.Index;
        }
        catch (CScriptException ex)
        {
            script.Errors.Add(ex);
            index = SkipUntilSemicolon(script, index);
        }
    }
    
    private bool MatchSignature(CScript script, int index, IReadOnlyList<CTokenType> types)
    {
        if (index >= script.Tokens.Count)
            return false;

        if (script.Tokens.Count - index < types.Count)
            return false;

        for (var i = 0; i < types.Count; i++)
        {
            var type = types[i];
            
            if (type == CTokenType.NULL) continue;
            
            if (script.Tokens[index + i].Type != type)
                return false;
        }

        return true;
    }

    private static int SkipUntilSemicolon(CScript script, int index)
    {
        while (index < script.Tokens.Count && script.Tokens[index].Type != CTokenType.SEMICOLON)
            index++;

        index++;

        return index;
    }
}
