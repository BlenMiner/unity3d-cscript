using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Riten.CScript.Native;
using Riten.CScript.Runtime;

[Serializable]
public class CScript : ScriptableObject
{
    [SerializeField] string m_CScriptSourceCode;
    [SerializeField] string m_savePath;
    [SerializeField] string m_dynamicUID;
    [SerializeField] CTLexer m_lexer = new ();
    [SerializeField] Instruction[] m_compiled;

    CTRoot m_rootNode;

    public string SourceCode
    {
        get => m_CScriptSourceCode;
        set => m_CScriptSourceCode = value;
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
        
        File.WriteAllText(m_savePath, m_CScriptSourceCode);
    }
    
    public void UpdateDynamicUID(string uid)
    {
        m_dynamicUID = uid;
    }

    public void Setup(string sourceCode, string savePath = null)
    {
        m_savePath = savePath;
        m_CScriptSourceCode = sourceCode;
        OnCodeUpdated();
    }

    private void OnCodeUpdated()
    {
        LineCount = m_CScriptSourceCode.Split('\n').Length;
        m_lexer.Parse(m_CScriptSourceCode);

        ParseRootNode();
        Compile();
    }

    private void ParseRootNode()
    {
        m_rootNode ??= new CTRoot();
        m_rootNode.Parse(m_lexer.Tokens);
    }

    private void Compile()
    {
        m_compiled = CTCompiler.Compile(m_rootNode);
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