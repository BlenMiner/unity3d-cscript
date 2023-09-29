//#undef UNITY_EDITOR
//#define UNITY_WEBGL

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Riten.CScript.Native
{
    public static class Interoperability
    {
        static IntPtr nativeLibraryPtr;

        delegate int InitializeJumpTableDelegate();
        
        delegate int ValidateOpcodeDelegate(int index, string name);
        
        delegate IntPtr GetOpcodeNameDelegate(int index);
        
        public delegate long ExecuteInstructionsDelegate([In] Instruction[] arr, int length);
        
        public delegate long ExecuteInstructionsPtrDelegate(IntPtr arr, int length);
        
        public delegate IntPtr CreateProgramDelegate([In] Instruction[] arr, int length);
        
        public delegate void FreeProgramDelegate(IntPtr program);
        
        public delegate long ExecuteProgramDelegate(IntPtr program);

        // ReSharper disable once UnusedMember.Local
        private const string DLL_NAME =
#if !UNITY_EDITOR && (UNITY_WEBGL || UNITY_IPHONE)
            "__Internal"; 
#else 
            "Interpreter";
#endif

#if !UNITY_EDITOR || PLUGIN_IMP
        [DllImport (DLL_NAME, EntryPoint="InitializeJumpTable")]
        private static extern int InitializeJumpTable();
        
        [DllImport(DLL_NAME, EntryPoint="ValidateOpcode")]
        private static extern int ValidateOpcode(int index, string name);
        
        [DllImport(DLL_NAME, EntryPoint="GetOpcodeName")]
        private static extern IntPtr GetOpcodeName(int index);
        
        [DllImport(DLL_NAME, EntryPoint="CreateProgram")]
        public static extern IntPtr CreateProgram([In] Instruction[] arr, int length);
        
        [DllImport(DLL_NAME, EntryPoint="FreeProgram")]
        public static extern void FreeProgram(IntPtr program);
        
        [DllImport(DLL_NAME, EntryPoint="ExecuteProgram")]
        public static extern long ExecuteProgram(IntPtr program);
#endif

        private static int OPCODE_COUNT;
        
        public static bool IsInitialized => nativeLibraryPtr != IntPtr.Zero;

        static InitializeJumpTableDelegate InitializeJumpTableFunc;
        
        static ValidateOpcodeDelegate ValidateOpcodeFunc;
        
        static GetOpcodeNameDelegate GetOpcodeNameFunc;
        
        public static CreateProgramDelegate CreateProgramFunc;
        
        public static FreeProgramDelegate FreeProgramFunc;
        
        public static ExecuteProgramDelegate ExecuteProgramFunc;
                    
#if UNITY_EDITOR
        static void ModeChangedArgs(UnityEditor.PlayModeStateChange mode)
        {
            if (mode == UnityEditor.PlayModeStateChange.EnteredEditMode)
            {
                Dispose();
                UnityEditor.EditorApplication.playModeStateChanged -= ModeChangedArgs;
            }
        }
#endif

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += ModeChangedArgs;
#endif

            Dispose();
            
#if UNITY_EDITOR && !PLUGIN_IMP
            if (nativeLibraryPtr != IntPtr.Zero) return;

            nativeLibraryPtr = Native.LoadLibrary("Assets/CScript/Plugins/Interpreter.dll");

            if (nativeLibraryPtr == IntPtr.Zero)
                Debug.LogError("Failed to load native library");
            else LoadFunctions();
#else
            InitializeJumpTableFunc = InitializeJumpTable;
            ValidateOpcodeFunc = ValidateOpcode;
            GetOpcodeNameFunc = GetOpcodeName;
            CreateProgramFunc = CreateProgram;
            FreeProgramFunc = FreeProgram;
            ExecuteProgramFunc = ExecuteProgram;
#endif
            Warmup();
        }
        
        static void Warmup()
        {
            OPCODE_COUNT = InitializeJumpTableFunc();

            var opcodes = Enum.GetNames(typeof(Opcodes));

            if (opcodes.Length != OPCODE_COUNT)
            {
                Debug.LogError("Failed to validate opcodes, the count doesn't match the native count.");
                PrintWhatEnumShouldBe();
            }
            else
            {
                for (int i = 0; i < opcodes.Length; i++)
                {
                    if (ValidateOpcodeFunc(i, opcodes[i]) == 0)
                    {
                        Debug.LogError($"Failed to validate opcode {opcodes[i]}, it doesn't match the native opcode " +
                                       $"{Marshal.PtrToStringAnsi(GetOpcodeNameFunc(i))}");
                        PrintWhatEnumShouldBe();           
                        break;
                    }
                }
            }
        }

        static void PrintWhatEnumShouldBe()
        {
            string whatEnumShouldBe = "public enum Opcodes : int\n{\n";
                    
            for (int j = 0; j < OPCODE_COUNT; j++)
            {
                whatEnumShouldBe += $"    {Marshal.PtrToStringAnsi(GetOpcodeNameFunc(j))},\n";
            }

            whatEnumShouldBe += "}";
                    
            Debug.LogError(whatEnumShouldBe);
        }
#if UNITY_EDITOR && !PLUGIN_IMP
        static void LoadFunctions()
        {
            InitializeJumpTableFunc = Native.GetFunction<InitializeJumpTableDelegate>(nativeLibraryPtr,
                "InitializeJumpTable");
            
            ValidateOpcodeFunc = Native.GetFunction<ValidateOpcodeDelegate>(nativeLibraryPtr,
                "ValidateOpcode");
            
            GetOpcodeNameFunc = Native.GetFunction<GetOpcodeNameDelegate>(nativeLibraryPtr,
                "GetOpcodeName");
                
            CreateProgramFunc = Native.GetFunction<CreateProgramDelegate>(nativeLibraryPtr, "CreateProgram");
            
            FreeProgramFunc = Native.GetFunction<FreeProgramDelegate>(nativeLibraryPtr, "FreeProgram");
            
            ExecuteProgramFunc = Native.GetFunction<ExecuteProgramDelegate>(nativeLibraryPtr, "ExecuteProgram");
        }
#endif
 
        public static void Dispose()
        {
#if UNITY_EDITOR && !PLUGIN_IMP
            if (nativeLibraryPtr == IntPtr.Zero) return;

            if (!Native.FreeLibrary(nativeLibraryPtr))
                Debug.LogError("Failed to unload native library");

            nativeLibraryPtr = IntPtr.Zero;
#endif
        }
    }
}