using CScript;
using UnityEngine;

public class Bruh : MonoBehaviour
{
    const int LOOP_COUNT = 1_000;

    CScriptProgram m_program;
    CScriptCompiled m_code;
    
    private void Awake()
    {
        var stack = new CScriptStack();
        m_code = new CScriptCompiled();
        m_program = new CScriptProgram(m_code, stack);
        
        m_code.Add(Opcodes.PUSH, 0);
        m_code.Add(Opcodes.MOVE_TO_REGISTER, 0);
        m_code.Add(Opcodes.PUSH, LOOP_COUNT);
        
        m_code.Add(Opcodes.JMP_IF_ZERO, 6);
        
        m_code.Add(Opcodes.ADD_CONSTANT, -1);
        
        m_code.Add(Opcodes.LOAD_REGISTER, 0);
        m_code.Add(Opcodes.ADD_CONSTANT, 2);
        m_code.Add(Opcodes.MOVE_TO_REGISTER, 0);
        
        m_code.Add(Opcodes.JMP, -5);
        
        m_code.Add(Opcodes.POP);
        m_code.Add(Opcodes.LOAD_REGISTER, 0);
        m_code.Add(Opcodes.POP);

        m_code.FinalizeCode();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            m_program.Stack.IP = 0;
            m_program.RunFinalized();
            Debug.Log(m_program.Stack.Operand);
        }
    }
}
