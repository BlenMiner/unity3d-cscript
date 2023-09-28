/*
 * Native dll invocation helper by Francis R. Griffiths-Keam
 * www.runningdimensions.com/blog
 */
 
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CScript.Native
{
    public static class Native
    {
        public static T Invoke<T, T2>(IntPtr library, params object[] pars)
        {
            var funcPtr = GetProcAddress(library, typeof(T2).Name);
            if (funcPtr == IntPtr.Zero)
            {
                Debug.LogWarning("Could not gain reference to method address.");
                return default(T);
            }

            var func = Marshal.GetDelegateForFunctionPointer(GetProcAddress(library, typeof(T2).Name), typeof(T2));
            return (T)func.DynamicInvoke(pars);
        }

        public static T Invoke<T, T2>(IntPtr library)
        {
            var funcPtr = GetProcAddress(library, typeof(T2).Name);
            if (funcPtr == IntPtr.Zero)
            {
                Debug.LogWarning("Could not gain reference to method address.");
                return default;
            }

            var func = Marshal.GetDelegateForFunctionPointer(GetProcAddress(library, typeof(T2).Name), typeof(T2));
            return (T)func.DynamicInvoke(Array.Empty<object>());
        }

        public static T GetFunction<T>(IntPtr library, in string functionName) where T : Delegate
        {
            var funcPtr = GetProcAddress(library, functionName);
            if (funcPtr == IntPtr.Zero)
            {
                Debug.LogWarning("Could not gain reference to method address.");
                return default;
            }

            return Marshal.GetDelegateForFunctionPointer<T>(GetProcAddress(library, functionName));
        }

        public static void Invoke<T>(IntPtr library, params object[] pars)
        {
            var funcPtr = GetProcAddress(library, typeof(T).Name);
            if (funcPtr == IntPtr.Zero)
            {
                Debug.LogWarning("Could not gain reference to method address.");
                return;
            }

            var func = Marshal.GetDelegateForFunctionPointer(funcPtr, typeof(T));
            func.DynamicInvoke(pars);
        }

        [DllImport("kernel32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
    }
}