namespace CScript
{
    public static class CScriptFlowControlInst
    {
        public static void JMP(this CScriptStack stack)
        {
            stack.IP = stack.PopInt();
        }
        
        public static void JMP_IF_TRUE(this CScriptStack stack)
        {
            var ip = stack.PopInt();

            if (stack.PopByte() != 0) stack.IP = ip;
        }
        
        public static void JMP_IF_FALSE(this CScriptStack stack)
        {
            var ip = stack.PopInt();
            
            if (stack.PopByte() == 0) stack.IP = ip;
        }
        
        public static void CALL(this CScriptStack stack)
        {
            var ip = stack.PopInt();
            
            stack.PushInt(stack.IP);
            stack.IP = ip;
        }
        
        public static void RET(this CScriptStack stack)
        {
            stack.IP = stack.PopInt();
        }
    }
}