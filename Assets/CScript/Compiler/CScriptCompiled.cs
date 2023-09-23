using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CScript
{
    public sealed class CScriptCompiled
    {
        public readonly List<CScriptInstruction> Instructions;
        
        public CScriptInstruction[] InstructionsArray;
        
        public CScriptCompiled()
        {
            Instructions = new List<CScriptInstruction>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Add(Opcodes opcode, float value)
        {
            Instructions.Add(new CScriptInstruction(opcode, *(long*)&value));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Add(Opcodes opcode, double value)
        {
            Instructions.Add(new CScriptInstruction(opcode, *(long*)&value));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Opcodes opcode, long value)
        {
            Instructions.Add(new CScriptInstruction(opcode, value));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Opcodes opcode, ulong value)
        {
            Instructions.Add(new CScriptInstruction(opcode, (long)value));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Opcodes opcode)
        {
            Instructions.Add(new CScriptInstruction(opcode));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FinalizeCode()
        {
            InstructionsArray = Instructions.ToArray();
        }
    }
}