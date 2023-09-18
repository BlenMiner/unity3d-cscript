using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("CScript/CScript Dynamic")]
public class CScriptDynamic : MonoBehaviour
{
    public const string DEFAULT_CODE = "69 + 420 * 3";
    static readonly Dictionary<CScriptUID, CScriptDynamic> s_scripts = new();

    [SerializeField, HideInInspector] private CScriptUID m_uid;
    [SerializeReference] private CScriptInline m_sourceCode;
    
    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(m_uid.Value))
            m_uid = new CScriptUID(Guid.NewGuid().ToString());
        
        if (s_scripts.TryGetValue(m_uid, out var script))
        {
            if (script == this)
                return;
            
            m_uid = new CScriptUID(Guid.NewGuid().ToString());
            var newScript = ScriptableObject.CreateInstance<CScript>();
            newScript.Setup(m_sourceCode.Script.SourceCode);
            m_sourceCode = new CScriptInline(newScript);
        }
        else
        {
            s_scripts.Add(m_uid, this);
        }
    }
}
