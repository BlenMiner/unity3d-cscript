using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CScript.Native
{
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public struct Instruction
    {
        public int Opcode;
        public int Reg;
        public int Reg2;
        public long Operand;
        public long Operand2;
        
        public Instruction(Opcodes opcode)
        {
            Opcode = (int)opcode;
            Operand = default;
            Reg = default;
            Reg2 = default;
            Operand2 = default;
        }
        
        public Instruction(Opcodes opcode, long operand)
        {
            Opcode = (int)opcode;
            Operand = operand;
            Reg = default;
            Reg2 = default;
            Operand2 = default;
        }
        
        public Instruction(Opcodes opcode, Registers register)
        {
            Opcode = (int)opcode;
            Operand = default;
            Reg = (int)register;
            Reg2 = default;
            Operand2 = default;
        }
        
        public Instruction(Opcodes opcode, Registers register, long operand)
        {
            Opcode = (int)opcode;
            Operand = operand;
            Reg = (int)register;
            Reg2 = default;
            Operand2 = default;
        }
        
        public Instruction(Opcodes opcode, Registers a, Registers b)
        {
            Opcode = (int)opcode;
            Operand = default;
            Reg = (int)a;
            Reg2 = (int)b;
            Operand2 = default;
        }
    }
}