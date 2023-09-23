using System.Reflection;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public static class CScriptCompiler
{
    [InitializeOnLoadMethod]
    static void Init()
    {
        // Compile();        
    }

    [MenuItem("CScript/Compile")]
    static void Compile()
    {
        var scripts = AssetDatabase.FindAssets("t:CScript");

        for (var i = 0; i < scripts.Length; i++)
        {
            var guid = scripts[i];
            var path = AssetDatabase.GUIDToAssetPath(guid);
            Debug.Log(path);
        }


        string fileName = "DynamicAssembly.dll";
        // Create an assembly name
        var assemblyName = new AssemblyName(fileName);

        // Create the dynamic assembly and module
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule(fileName);
        
        // Create a dynamic type
        var typeBuilder = moduleBuilder.DefineType("DynamicType", TypeAttributes.Public);

        // Define a simple method that will return 42
        var methodBuilder = typeBuilder.DefineMethod("GetAnswer", MethodAttributes.Public | MethodAttributes.Static, typeof(int), null);
        var il = methodBuilder.GetILGenerator();
        il.Emit(OpCodes.Ldc_I4, Random.Range(0, 69));
        il.Emit(OpCodes.Ret);
        
        var typeInfo = typeBuilder.CreateTypeInfo();
        
        var method = typeInfo.GetMethod("GetAnswer");
        
        Debug.Log(method!.Invoke(null, null));

        // assemblyBuilder.Save($"Assets/Plugins/CScript/{fileName}");
    }
}
