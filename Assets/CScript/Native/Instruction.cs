using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Riten.CScript.Native
{
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public struct Instruction
    {
        public int Opcode;
        public long Operand;
        public long Operand2;
        public long Operand3;
        public long Operand4;

        public Instruction(Opcodes opcode)
        {
            Opcode = (int)opcode;
            Operand = default;
            Operand2 = default;
            Operand3 = default;
            Operand4 = default;
        }
        
        public Instruction(Opcodes opcode, long operand)
        {
            Opcode = (int)opcode;
            Operand = operand;
            Operand2 = default;
            Operand3 = default;
            Operand4 = default;
        }
        
        public Instruction(Opcodes opcode, long operand, long operand2)
        {
            Opcode = (int)opcode;
            Operand = operand;
            Operand2 = operand2;
            Operand3 = default;
            Operand4 = default;
        }
        
        public Instruction(Opcodes opcode, long operand, long operand2, long operand3)
        {
            Opcode = (int)opcode;
            Operand = operand;
            Operand2 = operand2;
            Operand3 = operand3;
            Operand4 = default;
        }
        
        public Instruction(Opcodes opcode, long operand, long operand2, long operand3, long operand4)
        {
            Opcode = (int)opcode;
            Operand = operand;
            Operand2 = operand2;
            Operand3 = operand3;
            Operand4 = operand4;
        }
    }
}