using UnityEngine;
using UnityEditor.AssetImporters;

[ScriptedImporter(1, "ct")]
public class CScriptImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var script = ScriptableObject.CreateInstance<CScript>();
        script.Setup(System.IO.File.ReadAllText(ctx.assetPath), ctx.assetPath);
        
        ctx.AddObjectToAsset("script", script);
        ctx.SetMainObject(script);
    }
}
