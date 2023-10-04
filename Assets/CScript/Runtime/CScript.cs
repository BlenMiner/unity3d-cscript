using System;
using System.Collections.Generic;
using System.IO;
using Riten.CScript.Compiler;
using UnityEngine;
using Riten.CScript.Native;

[System.Serializable]
public struct CScriptFunction
{
    public string Name;
    public int Address;
}

[Serializable]
public class CScript : ScriptableObject
{
    [SerializeField] string m_CScriptSourceCode;
    [SerializeField] string m_savePath;
    [SerializeField] string m_dynamicUID;
    [SerializeField] CTLexer m_lexer = new ();
    [SerializeField] Instruction[] m_compiled;
    [SerializeField] List<CScriptFunction> m_functions;
    
    public Instruction[] Compiled => m_compiled;

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
        m_rootNode.Errors.Clear();
        m_rootNode.Parse(m_lexer.Tokens);
    }

    private void Compile()
    {
        var c = new CTCompiler(m_rootNode);
        m_compiled = c.Compile();

        m_functions ??= new List<CScriptFunction>();
        m_functions.Clear();
        
        foreach (var func in c.GlobalScope.Functions)
        {
            m_functions.Add(new CScriptFunction
            {
                Name = func.Key,
                Address = func.Value.FunctionPtr
            });
        }
    }

    public int GetFunctionPtr(string function)
    {
        for (int i = 0; i < m_functions.Count; i++)
        {
            if (m_functions[i].Name == function)
                return m_functions[i].Address;
        }

        return m_compiled.Length;
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