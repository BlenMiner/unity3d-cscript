using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("CScript/CScript Dynamic")]
public class CScriptDynamic : MonoBehaviour
{
    public const string DEFAULT_CODE = "69 + 420 * 3";
    static readonly Dictionary<CScriptUID, CScriptDynamic> s_scripts = new();
    
    [SerializeField, HideInInspector] private CScriptUID m_scriptUID;
    [SerializeReference] private CScriptInline m_sourceCode;
    
    private void OnValidate()
    {
        if (m_sourceCode == null) return;
        
        if (string.IsNullOrWhiteSpace(m_scriptUID.Value))
        {
            m_scriptUID = new CScriptUID(Guid.NewGuid().ToString());
            
            if (m_sourceCode != null && m_sourceCode.Script)
                m_sourceCode.Script.UpdateDynamicUID(m_scriptUID.Value);
        }

        if (s_scripts.TryGetValue(m_scriptUID, out var script))
        {
            if (script == this)
                return;
            
            m_scriptUID = new CScriptUID(Guid.NewGuid().ToString());
            var newScript = ScriptableObject.CreateInstance<CScript>();
            
            newScript.Setup(m_sourceCode == null ? DEFAULT_CODE : m_sourceCode.Script.SourceCode);
            newScript.UpdateDynamicUID(m_scriptUID.Value);
            
            m_sourceCode = new CScriptInline(newScript);
        }
        else
        {
            s_scripts.Add(m_scriptUID, this);
        }
    }
}
