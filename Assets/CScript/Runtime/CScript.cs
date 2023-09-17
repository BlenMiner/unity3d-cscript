using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class CScript : ScriptableObject
{
    [SerializeField] string m_sourceCode;
    [SerializeField] string m_savePath;
    [SerializeField] CTLexer m_lexer = new ();

    public List<CScriptException> Errors { get; private set; } = new();
    
    CTRoot m_rootNode;

    public string SourceCode
    {
        get => m_sourceCode;
        set => m_sourceCode = value;
    }
    
    public int LineCount { get; private set; }

    public CTRoot RootNode
    {
        get
        {
            if (m_rootNode == null )
                ParseRootNode();
            return m_rootNode;
        }
    }
    
    public IReadOnlyList<CToken> Tokens => m_lexer.Tokens;
    public string SavePath => m_savePath;

    public void Save()
    {
        OnCodeUpdated();

        if (string.IsNullOrWhiteSpace(m_savePath))
            return;
        
        File.WriteAllText(m_savePath, m_sourceCode);
    }

    public void Setup(string sourceCode, string savePath = null)
    {
        m_savePath = savePath;
        m_sourceCode = sourceCode;
        OnCodeUpdated();
    }

    private void OnCodeUpdated()
    {
        LineCount = m_sourceCode.Split('\n').Length;
        m_lexer.Parse(m_sourceCode);

        ParseRootNode();
    }

    private void ParseRootNode()
    {
        Errors.Clear();
        
        m_rootNode ??= new CTRoot();
        m_rootNode.Parse(this);
    }
}

[Serializable]
public class CScriptInline
{
    [SerializeField] CScript m_script;

    public CScript Script => m_script;
    
    public CScriptInline(CScript script)
    {
        m_script = script;
    }
}