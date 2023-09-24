using System.Runtime.CompilerServices;

namespace CScript
{
    public static unsafe class CScriptFlowControlInst
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void JMP(this CScriptStack stack)
        {
            var offset = stack.Operand - 1;
            stack.IP += *(int*)&offset;
        }
        
        public static void JMP_IF_TRUE(this CScriptStack stack)
        {
            if (stack.STACK[stack.SP++] != 0)
            {
                var offset = stack.Operand - 1;
                stack.IP += *(int*)&offset;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void JMP_IF_ZERO(this CScriptStack stack)
        {
            if (stack.STACK[stack.SP] == 0)
                stack.IP += (int)stack.Operand - 1;

            /*var shouldJump = stack.STACK[stack.SP] == 0;
            var offsetMult = *(byte*)&shouldJump;
            var currentMult = 1 - offsetMult;
            var offset = (int)stack.Operand - 1;
            stack.IP = stack.IP * currentMult + (stack.IP + offset) * offsetMult;*/
        }
        
        public static void JMP_IF_FALSE(this CScriptStack stack)
        {
            if (stack.STACK[stack.SP++] != 0)
                stack.IP += (int)stack.Operand - 1;
        }
        
        public static void CALL(this CScriptStack stack)
        {
            stack.STACK[--stack.SP] = stack.IP;
            stack.IP = (int)stack.Operand;
        }
        
        public static void RET(this CScriptStack stack)
        {
            stack.IP = (int)stack.STACK[stack.SP++];
        }
    }
}