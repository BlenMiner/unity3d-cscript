using System;
using Riten.CScript.Native;
using UnityEngine;
using UnityEngine.Profiling;

public class Bruh : MonoBehaviour
{
    [SerializeField] CScript m_script;
    
    const int LOOP_COUNT = 1000000;
    
    IntPtr m_scriptPtr;
    int m_programLength;

    private void Start()
    {
        m_scriptPtr = Interoperability.CreateProgramFunc(m_script.Compiled, m_script.Compiled.Length);
    }
    
    long fib(long n)
    {
        if (n <= 1)
        {
            return n;
        }

        return fib(n + -1) + fib(n + -2);
    }

    long test2()
    {
        return fib(30);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            Profiler.BeginSample("CScript C++");
            var res = Interoperability.ExecuteFunctionFunc(m_scriptPtr, m_script.GetFunctionPtr("test2"));
            Profiler.EndSample();
            
            Debug.Log(BitConverter.Int32BitsToSingle((int)res));

            Profiler.BeginSample("CScript C#");

            var a = test2();
            
            Profiler.EndSample();   
            Debug.Log(a);

        }
    }

    private void OnDestroy()
    {
        Interoperability.FreeProgramFunc(m_scriptPtr);
    }
}