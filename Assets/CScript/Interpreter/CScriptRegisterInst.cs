using System.Runtime.CompilerServices;

namespace CScript
{
    public static unsafe class CScriptRegisterInst
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void COPY_LONG(this CScriptStack stack)
        {
            var val = stack.STACK[stack.SP];
            stack.STACK[--stack.SP] = val;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MOVE_LONG(this CScriptStack stack)
        {
            /*fixed (long* s = stack.STACK, registers = stack.REGISTERS)
            {
                *(registers + stack.Operand) = *(s + stack.SP++);
            }  */
            
            stack.REGISTERS[stack.Operand] = stack.STACK[stack.SP++];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LOAD_LONG(this CScriptStack stack)
        {
            fixed (long* s = stack.STACK, registers = stack.REGISTERS)
            {
                *(s + --stack.SP) = *(registers + stack.Operand);
            }
            
            // stack.STACK[--stack.SP] = stack.REGISTERS[stack.Operand];
        }
    }
}