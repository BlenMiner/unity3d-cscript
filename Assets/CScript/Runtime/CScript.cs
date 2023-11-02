using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Riten.CScript.Compiler;
using UnityEngine;
using Riten.CScript.Native;
using Riten.CScript.TypeChecker;
using UnityEngine.Serialization;

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
    [SerializeField] CTLexer m_lexer = new ();
    [SerializeField] TypeSignatureResolver m_typeSolver;
    [SerializeField] List<CScriptFunction> m_functions;
    [SerializeField] Instruction[] m_compiled;

    public IReadOnlyList<CTError> Errors => m_lexer.Errors;

    public Instruction[] Compiled => m_compiled;

    CTRoot m_rootNode;

    public string SourceCode
    {
        get => m_CScriptSourceCode;
        set => m_CScriptSourceCode = value;
    }
    
    public int LineCount { get; private set; }

    public IReadOnlyList<CToken> Tokens => m_lexer.Tokens;
    
    public string SavePath => m_savePath;

    public void Save()
    {
        OnCodeUpdated();

        if (string.IsNullOrWhiteSpace(m_savePath))
            return;
        
        File.WriteAllText(m_savePath, m_CScriptSourceCode);
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

        if (string.IsNullOrEmpty(m_CScriptSourceCode)) return;
        
        LineCount = m_CScriptSourceCode.Split('\n').Length;
        m_lexer.Parse(m_CScriptSourceCode);

        ParseRootNode();
        
        try
        {
            Compile();
        }
        catch (CTLexerException e)
        {
            m_lexer.RegisterError(e.Error);
        }
    }

    private void ParseRootNode()
    {
        m_rootNode ??= new CTRoot();
        m_rootNode.Parse(m_lexer);
    }

    private void Compile()
    {
        m_typeSolver = new TypeSignatureResolver(m_rootNode);
        
        var c = new CTCompiler(m_rootNode, m_typeSolver);

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