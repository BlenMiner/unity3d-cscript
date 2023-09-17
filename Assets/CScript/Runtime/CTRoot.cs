using System.Collections.Generic;

[System.Serializable]
public class CTRoot : CTNode
{
    protected List<CTNode> m_children = new ();
    
    public IReadOnlyList<CTNode> Children => m_children;

    public CTRoot() : base(CTNodeType.Root) {}
    
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
            
            try
            {
                var response = CTNodeExpression.Parse(script, index);
                m_children.Add(response.Node);
                index = response.Index;
            }
            catch (CScriptException ex)
            {
                script.Errors.Add(ex);
                index = SkipUntilSemicolon(script, index);
            }
        }
    }

    private static int SkipUntilSemicolon(CScript script, int index)
    {
        while (index < script.Tokens.Count && script.Tokens[index].Type != CTokenType.SEMICOLON)
            index++;

        index++;

        return index;
    }
}
