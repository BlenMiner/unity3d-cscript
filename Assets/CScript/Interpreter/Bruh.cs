using CScript;
using UnityEngine;
using UnityEngine.Profiling;

public class Bruh : MonoBehaviour
{
    const int LOOP_COUNT = 100000;

    CScriptProgram m_program;
    CScriptCompiled m_code;
    
    private void Awake()
    {
        var stack = new CScriptStack();
        m_code = new CScriptCompiled();
        m_program = new CScriptProgram(m_code, stack);
        
        m_code.Add(Opcodes.PUSH, 0);
        m_code.Add(Opcodes.MOVE_TO_REGISTER, 0); // Move pushed int to R0 leaving stack empty
        m_code.Add(Opcodes.PUSH, LOOP_COUNT);
        m_code.Add(Opcodes.DUPLICATE);
        m_code.Add(Opcodes.PUSH, 0);
        m_code.Add(Opcodes.EQUAL);
        m_code.Add(Opcodes.JMP_IF_TRUE, 8); // Out of range = exit
        m_code.Add(Opcodes.PUSH, -1);
        m_code.Add(Opcodes.ADD);
        m_code.Add(Opcodes.LOAD_REGISTER, 0);
        m_code.Add(Opcodes.PUSH, 2);
        m_code.Add(Opcodes.ADD);
        m_code.Add(Opcodes.MOVE_TO_REGISTER, 0);
        m_code.Add(Opcodes.JMP, -10);
        m_code.Add(Opcodes.POP);
        m_code.Add(Opcodes.LOAD_REGISTER, 0);
        m_code.Add(Opcodes.POP);

        m_code.FinalizeCode();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("CScript For Loop");
            
            Profiler.BeginSample("CScript For Loop");
            m_program.Stack.IP = 0;
            m_program.RunFinalized();
            Profiler.EndSample();
        }
    }
}
