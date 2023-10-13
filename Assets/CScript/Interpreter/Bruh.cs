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

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            Profiler.BeginSample("CScript C++");
            var res = Interoperability.ExecuteFunctionFunc(m_scriptPtr, m_script.GetFunctionPtr("test2"));
            Profiler.EndSample();
            
            Debug.Log(res);

            Profiler.BeginSample("CScript C#");
            
            long a = 0;
            long b = 1;
     
            for (int i = 0; i < 1000000; ++i)
            {
                long tmp = a;
                a = b;
                b = tmp  + b;  
            } 

            /*long r1 = 1;
            long r2 = 0;
            long r0 = 0;
            
            for (int i = 0; i < LOOP_COUNT; i++)
            {
                r0 = r1 + r2;
                r2 = r1;
                r1 = r0;
            }*/
            Profiler.EndSample();   
            Debug.Log(a);

        }
    }

    private void OnDestroy()
    {
        Interoperability.FreeProgramFunc(m_scriptPtr);
    }
}