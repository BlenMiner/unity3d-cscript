namespace CScript
{
    public static class CScriptRegisterInst
    {
        public static void COPY_LONG(this CScriptStack stack)
        {
            var val = stack.STACK[stack.SP];
            stack.STACK[--stack.SP] = val;
        }
        
        public static void MOVE_LONG(this CScriptStack stack)
        {
            stack.REGISTERS[stack.Operand] = stack.STACK[stack.SP++];
        }
        
        public static void LOAD_LONG(this CScriptStack stack)
        {
            stack.STACK[--stack.SP] = stack.REGISTERS[stack.Operand];
        }
    }
}