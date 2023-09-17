using UnityEditor;
using UnityEngine;

public static class CScriptCreator
{
    [MenuItem("Assets/Create/CScript/New CScript", false, 100)]
    public static void CreateNewScript()
    {
        var template = Resources.Load<TextAsset>("CScriptTemplate");

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
            AssetDatabase.GetAssetPath(template), 
            "NewCScript.ct"
        );
    }
}
