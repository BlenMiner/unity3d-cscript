using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace CScript
{
    public sealed class CScriptProgram
    {
        private readonly CScriptCompiled m_compiledScript;
        private readonly CScriptStack m_stack;
        
        public CScriptStack Stack => m_stack;
        
        public CScriptProgram(CScriptCompiled compiledScript, CScriptStack stack)
        {
            CScriptOpcodeExtensions.Init();
            m_compiledScript = compiledScript;
            m_stack = stack;
        }
        
        public void Step()
        {
            var instruction = m_compiledScript.Instructions[m_stack.IP++];
            
            if (instruction.HasValue == 1) m_stack.Operand = instruction.Value;
            
            CScriptOpcodeExtensions.Execute(instruction.Opcode, m_stack);
        }
        
        public unsafe void RunFinalized()
        {
            Profiler.BeginSample("CScriptProgram.RunFinalized");
            
            if (m_compiledScript.InstructionsArray == null) 
                m_compiledScript.FinalizeCode();

            var instructions = m_compiledScript.InstructionsArray;
            int instructionSize = instructions!.Length;
            
            var intructionsSpan = (ReadOnlySpan<CScriptInstruction>)instructions;

            while (m_stack.IP < instructionSize)
            {
                var instruction = intructionsSpan[m_stack.IP++];

                m_stack.Operand = m_stack.Operand & instruction.HasValueInverse | 
                                  instruction.Value & instruction.HasValue;

                CScriptOpcodeExtensions.OP_CODE_IMPLEMENTATION[instruction.OpcodeIndex](m_stack);
            }

            Profiler.EndSample();
        }
        
        public void Run()
        {
            int instructionSize = m_compiledScript.Instructions.Count;
            while (m_stack.IP < instructionSize)
                Step();
        }
        
        public void RunWithErrorLogging(int maxSteps = 1000)
        {
            int steps = 0;
            while (m_stack.IP < m_compiledScript.Instructions.Count && steps < maxSteps)
            {
                int inst = m_stack.IP;

                try
                {
                    Step();
                }
                catch (Exception ex)
                {
                    var instruction = m_compiledScript.Instructions[inst];

                    Debug.LogError(instruction.HasValue == 1
                        ? $"Failed at {instruction.Opcode} {instruction.Value} (line {inst}): {ex.Message}"
                        : $"Failed at {instruction.Opcode} (line {inst}): {ex.Message}");

                    throw;
                }
                
                steps++;
            }
        }
    }
}