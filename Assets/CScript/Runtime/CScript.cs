using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Riten.CScript.Compiler;
using UnityEngine;
using Riten.CScript.Native;

[Serializable]
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
    [SerializeField] List<CTError> m_errors = new ();
    
    public List<CTError> Errors => m_errors;

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
        m_compiled = Array.Empty<Instruction>();
        Errors.Clear();

        if (string.IsNullOrEmpty(m_CScriptSourceCode))
        {
            Errors.Add(new CTError
            {
                Message = "No source code provided."
            });
            return;
        }
        
        LineCount = m_CScriptSourceCode.Split('\n').Length;
        m_lexer.Parse(m_CScriptSourceCode);

        ParseRootNode();
        Compile();
    }

    private void ParseRootNode()
    {
        m_rootNode ??= new CTRoot(ErrorCatcher);
        m_rootNode.Parse(m_lexer.Tokens);
    }

    private void ErrorCatcher(CTLexerException exception)
    {
        Errors.Add(exception.ToError());
    }

    private void Compile()
    {
        var c = new CTCompiler(m_rootNode);

        try
        {
            m_compiled = c.Compile();
        }
        catch (Exception e)
        {
            Errors.Add(new CTError
            {
                Message = e.Message
            });
            return;
        }

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

    [ContextMenu("Log Program")]
    public void LogProgram()
    {
        var sb = new StringBuilder();

        for (int i = 0; i < m_compiled.Length; i++)
        {
            var inst = m_compiled[i];
            sb.AppendLine($"Instruction(Opcodes::{(Opcodes)inst.Opcode}, {inst.Operand}, {inst.Operand2}, {inst.Operand3}, {inst.Operand4}),");
        }
        
        Debug.Log(sb.ToString());
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