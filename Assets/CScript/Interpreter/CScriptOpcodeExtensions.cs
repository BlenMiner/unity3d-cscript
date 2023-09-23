using System;

namespace CScript
{
    public static class CScriptOpcodeExtensions
    {
        private static Action<CScriptStack>[] OP_CODE_IMPLEMENTATION;
        
        static bool s_initialized;
        
        public static void Init()
        {
            if (s_initialized) return;
            
            s_initialized = true;
            OP_CODE_IMPLEMENTATION = new Action<CScriptStack>[(int) Opcodes.OPCODE_COUNT];
            
            OP_CODE_IMPLEMENTATION[(int) Opcodes.PUSH_BYTE] = CScriptStackIntructions.PushByte;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.PUSH_SBYTE] = CScriptStackIntructions.PushSByte;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.PUSH_SHORT] = CScriptStackIntructions.PushShort;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.PUSH_USHORT] = CScriptStackIntructions.PushUShort;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.PUSH_INT] = CScriptStackIntructions.PushInt;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.PUSH_UINT] = CScriptStackIntructions.PushUInt;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.PUSH_LONG] = CScriptStackIntructions.PushLong;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.PUSH_ULONG] = CScriptStackIntructions.PushULong;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.PUSH_FLOAT] = CScriptStackIntructions.PushFloat;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.PUSH_DOUBLE] = CScriptStackIntructions.PushDouble;
            
            OP_CODE_IMPLEMENTATION[(int) Opcodes.POP_BYTE] = CScriptStackIntructions.PopByte;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.POP_SBYTE] = CScriptStackIntructions.PopSByte;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.POP_SHORT] = CScriptStackIntructions.PopShort;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.POP_USHORT] = CScriptStackIntructions.PopUShort;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.POP_INT] = CScriptStackIntructions.PopInt;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.POP_UINT] = CScriptStackIntructions.PopUInt;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.POP_LONG] = CScriptStackIntructions.PopLong;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.POP_ULONG] = CScriptStackIntructions.PopULong;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.POP_FLOAT] = CScriptStackIntructions.PopFloat;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.POP_DOUBLE] = CScriptStackIntructions.PopDouble; 
            
            OP_CODE_IMPLEMENTATION[(int) Opcodes.ADD_BYTE] = CScriptMathInstructions.ADD_BYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.ADD_SBYTE] = CScriptMathInstructions.ADD_SBYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.ADD_SHORT] = CScriptMathInstructions.ADD_SHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.ADD_USHORT] = CScriptMathInstructions.ADD_USHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.ADD_INT] = CScriptMathInstructions.ADD_INT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.ADD_UINT] = CScriptMathInstructions.ADD_UINT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.ADD_LONG] = CScriptMathInstructions.ADD_LONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.ADD_ULONG] = CScriptMathInstructions.ADD_ULONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.ADD_FLOAT] = CScriptMathInstructions.ADD_FLOAT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.ADD_DOUBLE] = CScriptMathInstructions.ADD_DOUBLE;
            
            OP_CODE_IMPLEMENTATION[(int) Opcodes.SUB_BYTE] = CScriptMathInstructions.SUB_BYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.SUB_SBYTE] = CScriptMathInstructions.SUB_SBYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.SUB_SHORT] = CScriptMathInstructions.SUB_SHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.SUB_USHORT] = CScriptMathInstructions.SUB_USHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.SUB_INT] = CScriptMathInstructions.SUB_INT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.SUB_UINT] = CScriptMathInstructions.SUB_UINT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.SUB_LONG] = CScriptMathInstructions.SUB_LONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.SUB_ULONG] = CScriptMathInstructions.SUB_ULONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.SUB_FLOAT] = CScriptMathInstructions.SUB_FLOAT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.SUB_DOUBLE] = CScriptMathInstructions.SUB_DOUBLE;
            
            OP_CODE_IMPLEMENTATION[(int) Opcodes.MUL_BYTE] = CScriptMathInstructions.MUL_BYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.MUL_SBYTE] = CScriptMathInstructions.MUL_SBYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.MUL_SHORT] = CScriptMathInstructions.MUL_SHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.MUL_USHORT] = CScriptMathInstructions.MUL_USHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.MUL_INT] = CScriptMathInstructions.MUL_INT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.MUL_UINT] = CScriptMathInstructions.MUL_UINT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.MUL_LONG] = CScriptMathInstructions.MUL_LONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.MUL_ULONG] = CScriptMathInstructions.MUL_ULONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.MUL_FLOAT] = CScriptMathInstructions.MUL_FLOAT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.MUL_DOUBLE] = CScriptMathInstructions.MUL_DOUBLE;
            
            OP_CODE_IMPLEMENTATION[(int) Opcodes.DIV_BYTE] = CScriptMathInstructions.DIV_BYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.DIV_SBYTE] = CScriptMathInstructions.DIV_SBYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.DIV_SHORT] = CScriptMathInstructions.DIV_SHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.DIV_USHORT] = CScriptMathInstructions.DIV_USHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.DIV_INT] = CScriptMathInstructions.DIV_INT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.DIV_UINT] = CScriptMathInstructions.DIV_UINT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.DIV_LONG] = CScriptMathInstructions.DIV_LONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.DIV_ULONG] = CScriptMathInstructions.DIV_ULONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.DIV_FLOAT] = CScriptMathInstructions.DIV_FLOAT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.DIV_DOUBLE] = CScriptMathInstructions.DIV_DOUBLE;
            
            OP_CODE_IMPLEMENTATION[(int) Opcodes.JMP] = CScriptFlowControlInst.JMP;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.JMP_IF_TRUE] = CScriptFlowControlInst.JMP_IF_TRUE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.JMP_IF_FALSE] = CScriptFlowControlInst.JMP_IF_FALSE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.CALL] = CScriptFlowControlInst.CALL;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.RET] = CScriptFlowControlInst.RET;
            
            OP_CODE_IMPLEMENTATION[(int) Opcodes.EQUAL_BYTE] = CScriptEqualiyInst.EQUAL_BYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.EQUAL_SBYTE] = CScriptEqualiyInst.EQUAL_SBYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.EQUAL_SHORT] = CScriptEqualiyInst.EQUAL_SHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.EQUAL_USHORT] = CScriptEqualiyInst.EQUAL_USHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.EQUAL_INT] = CScriptEqualiyInst.EQUAL_INT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.EQUAL_UINT] = CScriptEqualiyInst.EQUAL_UINT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.EQUAL_LONG] = CScriptEqualiyInst.EQUAL_LONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.EQUAL_ULONG] = CScriptEqualiyInst.EQUAL_ULONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.EQUAL_FLOAT] = CScriptEqualiyInst.EQUAL_FLOAT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.EQUAL_DOUBLE] = CScriptEqualiyInst.EQUAL_DOUBLE;
            
            OP_CODE_IMPLEMENTATION[(int) Opcodes.NOT_EQUAL_BYTE] = CScriptEqualiyInst.NOT_EQUAL_BYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.NOT_EQUAL_SBYTE] = CScriptEqualiyInst.NOT_EQUAL_SBYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.NOT_EQUAL_SHORT] = CScriptEqualiyInst.NOT_EQUAL_SHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.NOT_EQUAL_USHORT] = CScriptEqualiyInst.NOT_EQUAL_USHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.NOT_EQUAL_INT] = CScriptEqualiyInst.NOT_EQUAL_INT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.NOT_EQUAL_UINT] = CScriptEqualiyInst.NOT_EQUAL_UINT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.NOT_EQUAL_LONG] = CScriptEqualiyInst.NOT_EQUAL_LONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.NOT_EQUAL_ULONG] = CScriptEqualiyInst.NOT_EQUAL_ULONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.NOT_EQUAL_FLOAT] = CScriptEqualiyInst.NOT_EQUAL_FLOAT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.NOT_EQUAL_DOUBLE] = CScriptEqualiyInst.NOT_EQUAL_DOUBLE;
            
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_THAN_BYTE] = CScriptEqualiyInst.GREATER_THAN_BYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_THAN_SBYTE] = CScriptEqualiyInst.GREATER_THAN_SBYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_THAN_SHORT] = CScriptEqualiyInst.GREATER_THAN_SHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_THAN_USHORT] = CScriptEqualiyInst.GREATER_THAN_USHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_THAN_INT] = CScriptEqualiyInst.GREATER_THAN_INT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_THAN_UINT] = CScriptEqualiyInst.GREATER_THAN_UINT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_THAN_LONG] = CScriptEqualiyInst.GREATER_THAN_LONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_THAN_ULONG] = CScriptEqualiyInst.GREATER_THAN_ULONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_THAN_FLOAT] = CScriptEqualiyInst.GREATER_THAN_FLOAT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_THAN_DOUBLE] = CScriptEqualiyInst.GREATER_THAN_DOUBLE;

            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_EQUAL_BYTE] = CScriptEqualiyInst.GREATER_EQUAL_BYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_EQUAL_SBYTE] = CScriptEqualiyInst.GREATER_EQUAL_SBYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_EQUAL_SHORT] = CScriptEqualiyInst.GREATER_EQUAL_SHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_EQUAL_USHORT] = CScriptEqualiyInst.GREATER_EQUAL_USHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_EQUAL_INT] = CScriptEqualiyInst.GREATER_EQUAL_INT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_EQUAL_UINT] = CScriptEqualiyInst.GREATER_EQUAL_UINT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_EQUAL_LONG] = CScriptEqualiyInst.GREATER_EQUAL_LONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_EQUAL_ULONG] = CScriptEqualiyInst.GREATER_EQUAL_ULONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_EQUAL_FLOAT] = CScriptEqualiyInst.GREATER_EQUAL_FLOAT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.GREATER_EQUAL_DOUBLE] = CScriptEqualiyInst.GREATER_EQUAL_DOUBLE;

            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_BYTE] = CScriptEqualiyInst.LESS_BYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_SBYTE] = CScriptEqualiyInst.LESS_SBYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_SHORT] = CScriptEqualiyInst.LESS_SHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_USHORT] = CScriptEqualiyInst.LESS_USHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_INT] = CScriptEqualiyInst.LESS_INT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_UINT] = CScriptEqualiyInst.LESS_UINT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_LONG] = CScriptEqualiyInst.LESS_LONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_ULONG] = CScriptEqualiyInst.LESS_ULONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_FLOAT] = CScriptEqualiyInst.LESS_FLOAT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_DOUBLE] = CScriptEqualiyInst.LESS_DOUBLE;

            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_EQUAL_BYTE] = CScriptEqualiyInst.LESS_EQUAL_BYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_EQUAL_SBYTE] = CScriptEqualiyInst.LESS_EQUAL_SBYTE;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_EQUAL_SHORT] = CScriptEqualiyInst.LESS_EQUAL_SHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_EQUAL_USHORT] = CScriptEqualiyInst.LESS_EQUAL_USHORT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_EQUAL_INT] = CScriptEqualiyInst.LESS_EQUAL_INT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_EQUAL_UINT] = CScriptEqualiyInst.LESS_EQUAL_UINT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_EQUAL_LONG] = CScriptEqualiyInst.LESS_EQUAL_LONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_EQUAL_ULONG] = CScriptEqualiyInst.LESS_EQUAL_ULONG;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_EQUAL_FLOAT] = CScriptEqualiyInst.LESS_EQUAL_FLOAT;
            OP_CODE_IMPLEMENTATION[(int) Opcodes.LESS_EQUAL_DOUBLE] = CScriptEqualiyInst.LESS_EQUAL_DOUBLE;
        }
        
        public static void Execute(this Opcodes opcode, CScriptStack stack)
            => OP_CODE_IMPLEMENTATION[(int) opcode].Invoke(stack);
    }
}
