namespace CScript
{
    public static class CScriptFlowControlInst
    {
        public static void JMP(this CScriptStack stack)
        {
            stack.IP += (int)stack.Operand - 1;
        }
        
        public static void JMP_IF_TRUE(this CScriptStack stack)
        {
            if (stack.Pop() != 0) stack.IP += (int)stack.Operand - 1;
        }
        
        public static void JMP_IF_FALSE(this CScriptStack stack)
        {
            if (stack.Pop() == 0) stack.IP += (int)stack.Operand - 1;
        }
        
        public static void CALL(this CScriptStack stack)
        {
            stack.Push(stack.IP);
            stack.IP = (int)stack.Operand;
        }
        
        public static void RET(this CScriptStack stack)
        {
            stack.IP = (int)stack.Pop();
        }
    }
}