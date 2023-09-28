namespace CScript
{
    public static class CScriptEqualiyInst
    {
        public static unsafe void EQUAL_ZERO(this CScriptStack stack)
        {
            var boolean = stack.STACK_RAW[stack.SP] == 0;
            stack.STACK_RAW[--stack.SP] = *(byte*)&boolean;
        }

        public static unsafe void NOT_EQUAL_ZERO(this CScriptStack stack)
        {
            var boolean = stack.STACK_RAW[stack.SP] != 0;
            stack.STACK_RAW[--stack.SP] = *(byte*)&boolean;
        }
        
        public static unsafe void EQUAL(this CScriptStack stack)
        {
            var boolean = stack.STACK_RAW[stack.SP++] == stack.STACK_RAW[stack.SP];
            stack.STACK_RAW[stack.SP] = *(byte*)&boolean;
        }

        public static void NOT_EQUAL(this CScriptStack stack)
        {
            stack.Push(stack.Pop() != stack.Pop() ? 1 : 0);
        }

        public static void GREATER_THAN(this CScriptStack stack)
        {
            stack.Push(stack.Pop() < stack.Pop() ? 1 : 0);
        }
        
        public static void GREATER_EQUAL(this CScriptStack stack)
        {
            stack.Push(stack.Pop() <= stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_THAN(this CScriptStack stack)
        {
            stack.Push(stack.Pop() > stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_EQUAL(this CScriptStack stack)
        {
            stack.Push(stack.Pop() >= stack.Pop() ? 1 : 0);
        }
    }
}