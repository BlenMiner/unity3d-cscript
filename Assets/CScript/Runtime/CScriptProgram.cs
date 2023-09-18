using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

public class CScriptProgram
{
    readonly CScript m_sourceCode;
    
    public CScriptProgram(CScript source)
    {
        m_sourceCode = source;
    }
    
    void Compile(string fileName)
    {
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
        il.Emit(OpCodes.Ldc_I4, 69);
        il.Emit(OpCodes.Ret);
        
        typeBuilder.CreateTypeInfo();

        assemblyBuilder.Save($"Assets/Plugins/CScript/{fileName}");

        DynamicType.GetAnswer();
    }
}
