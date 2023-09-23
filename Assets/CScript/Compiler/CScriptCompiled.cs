using System.Collections.Generic;

namespace CScript
{
    public sealed class CScriptCompiled
    {
        public readonly List<CScriptInstruction> Instructions;
        
        public CScriptCompiled()
        {
            Instructions = new List<CScriptInstruction>();
        }
        
        public unsafe void Add(Opcodes opcode, float value)
        {
            Instructions.Add(new CScriptInstruction(opcode, *(long*)&value));
        }
        
        public unsafe void Add(Opcodes opcode, double value)
        {
            Instructions.Add(new CScriptInstruction(opcode, *(long*)&value));
        }
        
        public void Add(Opcodes opcode, long value)
        {
            Instructions.Add(new CScriptInstruction(opcode, value));
        }
        
        public void Add(Opcodes opcode, ulong value)
        {
            Instructions.Add(new CScriptInstruction(opcode, (long)value));
        }
        
        public void Add(Opcodes opcode)
        {
            Instructions.Add(new CScriptInstruction(opcode));
        }
    }
}