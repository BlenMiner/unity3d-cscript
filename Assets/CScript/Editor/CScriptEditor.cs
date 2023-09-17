using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class CScriptEditor : EditorWindow
{
    [SerializeField] CScriptField m_field;
    [SerializeField] private string m_key;

    Vector2 m_scrollPosition;
    
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        var script = EditorUtility.InstanceIDToObject(instanceID) as CScript;
        
        if (script != null)
        {
            Open(AssetDatabase.GetAssetPath(instanceID));
            return true;
        }
        
        return false;
    }

    public override void DiscardChanges()
    {
        base.DiscardChanges();
        m_field.Save();
    }

    private void OnLostFocus()
    {
        m_field.Save();
    }

    public static void Open(string fullPath)
    {
        var script = AssetDatabase.LoadAssetAtPath<CScript>(fullPath);
        
        var editors = Resources.FindObjectsOfTypeAll<CScriptEditor>();

        if (!script) return;
        
        var fileInfo = new FileInfo(fullPath);
        var fullPathStr = fileInfo.FullName;

        for (int i = 0; i < editors.Length; ++i)
        {
            if (editors[i].m_key == fullPathStr)
            {
                editors[i].Focus();
                return;
            }
        }
        
        var w = CreateWindow<CScriptEditor>($"{fileInfo.Name}");
        w.m_field = new CScriptField(script);
        w.m_key = fullPathStr;
    } 

    void OnGUI()
    {
        var windowSize = position.size;
        
        var rect = new Rect(Vector2.one, windowSize - new Vector2(2, 2));
        m_field?.OnGUI(rect);
    }
}

