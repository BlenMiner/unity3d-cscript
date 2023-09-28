using System;
using CScript.Native;
using UnityEngine;
using UnityEngine.Profiling;

public class Bruh : MonoBehaviour
{
    const int LOOP_COUNT = 1000000;
    
    IntPtr m_program;
    int m_programLength;

    private void Start()
    {
        /*var program = new[]
        {
            new Instruction(Opcodes.PUSH_CONST, LOOP_COUNT),
            new Instruction(Opcodes.PUSH_CONST, 0L),
            new Instruction(Opcodes.POP_TO_REG, Registers.R0),
            
            new Instruction(Opcodes.REPEAT),
            new Instruction(Opcodes.ADD_CONST_TO_REG, Registers.R0, 2),
            new Instruction(Opcodes.REPEAT_END),
            
            new Instruction(Opcodes.PUSH_REG, Registers.R0),
        };   */
        
        var program = new[]
        {
            new Instruction(Opcodes.PUSH_CONST, 0L),
            new Instruction(Opcodes.PUSH_CONST, 1L),
            new Instruction(Opcodes.POP_TO_REG, Registers.R1),
            new Instruction(Opcodes.POP_TO_REG, Registers.R2),
            
            new Instruction(Opcodes.REPEAT_CONST, LOOP_COUNT),
            
            new Instruction(Opcodes.ADD_REG_TO_REG, Registers.R1, Registers.R2),
            new Instruction(Opcodes.SWAP_REG_REG, Registers.R2, Registers.R1),
            
            new Instruction(Opcodes.REPEAT_END),
            new Instruction(Opcodes.PUSH_REG, Registers.R1),
        };

        m_program = Interoperability.CreateProgramFunc(program, program.Length);

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Profiler.BeginSample("CScript C++");
            long res = Interoperability.ExecuteProgramFunc(m_program);
            Profiler.EndSample();

            Debug.Log(res);
            
            Profiler.BeginSample("CScript C#");
            
            /*long r0 = 0;

            for (int i = 0; i < LOOP_COUNT; i++)
            {
                r0 += 2;
            }  */
            long r1 = 1;
            long r2 = 0;
            long r0 = 0;
            
            for (int i = 0; i < LOOP_COUNT; i++)
            {
                r0 = r1 + r2;
                r2 = r1;
                r1 = r0;
            }
            
            Profiler.EndSample();   
            Debug.Log(r0);
        }
    }

    private void OnDestroy()
    {
        Interoperability.FreeProgramFunc(m_program);
    }
}