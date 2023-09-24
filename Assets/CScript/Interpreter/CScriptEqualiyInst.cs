namespace CScript
{
    public static class CScriptEqualiyInst
    {
        public static unsafe void EQUAL_ZERO(this CScriptStack stack)
        {
            var boolean = stack.STACK[stack.SP] == 0;
            stack.STACK[--stack.SP] = *(byte*)&boolean;
        }

        public static unsafe void NOT_EQUAL_ZERO(this CScriptStack stack)
        {
            var boolean = stack.STACK[stack.SP] != 0;
            stack.STACK[--stack.SP] = *(byte*)&boolean;
        }
        
        public static unsafe void EQUAL(this CScriptStack stack)
        {
            var boolean = stack.STACK[stack.SP++] == stack.STACK[stack.SP];
            stack.STACK[stack.SP] = *(byte*)&boolean;
        }

        public static void NOT_EQUAL(this CScriptStack stack)
        {
            stack.Push(stack.Pop() != stack.Pop() ? 1 : 0);
        }

        public static void GREATER_THAN_BYTE(this CScriptStack stack)
        {
            stack.Push((byte)stack.Pop() < (byte)stack.Pop() ? 1 : 0);
        }

        public static void GREATER_THAN_SBYTE(this CScriptStack stack)
        {
            stack.Push((sbyte)stack.Pop() < (sbyte)stack.Pop() ? 1 : 0);
        }
        
        public static void GREATER_THAN_SHORT(this CScriptStack stack)
        {
            stack.Push((short)stack.Pop() < (short)stack.Pop() ? 1 : 0);
        }
        
        public static void GREATER_THAN_USHORT(this CScriptStack stack)
        {
            stack.Push((ushort)stack.Pop() < (ushort)stack.Pop() ? 1 : 0);
        }
        
        public static void GREATER_THAN_INT(this CScriptStack stack)
        {
            stack.Push((int)stack.Pop() < (int)stack.Pop() ? 1 : 0);
        }
        
        public static void GREATER_THAN_UINT(this CScriptStack stack)
        {
            stack.Push((uint)stack.Pop() < (uint)stack.Pop() ? 1 : 0);
        }
        
        public static void GREATER_THAN_LONG(this CScriptStack stack)
        {
            stack.Push(stack.Pop() < stack.Pop() ? 1 : 0);
        }
        
        public static void GREATER_THAN_ULONG(this CScriptStack stack)
        {
            stack.Push((ulong)stack.Pop() < (ulong)stack.Pop() ? 1 : 0);
        }
        
        public static unsafe void GREATER_THAN_FLOAT(this CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push(*(float*)&a < *(float*)&b ? 1 : 0);
        }
        
        public static unsafe void GREATER_THAN_DOUBLE(this CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push(*(double*)&a < *(double*)&b ? 1 : 0);
        }
        
        public static void GREATER_EQUAL_BYTE(this CScriptStack stack)
        {
            stack.Push((byte)stack.Pop() <= (byte)stack.Pop() ? 1 : 0);
        }
        
        public static void GREATER_EQUAL_SBYTE(this CScriptStack stack)
        {
            stack.Push((sbyte)stack.Pop() <= (sbyte)stack.Pop() ? 1 : 0);
        }
        
        public static void GREATER_EQUAL_SHORT(this CScriptStack stack)
        {
            stack.Push((short)stack.Pop() <= (short)stack.Pop() ? 1 : 0);
        }
        
        public static void GREATER_EQUAL_USHORT(this CScriptStack stack)
        {
            stack.Push((ushort)stack.Pop() <= (ushort)stack.Pop() ? 1 : 0);
        }
        
        public static void GREATER_EQUAL_INT(this CScriptStack stack)
        {
            stack.Push((int)stack.Pop() <= (int)stack.Pop() ? 1 : 0);
        }
        
        public static void GREATER_EQUAL_UINT(this CScriptStack stack)
        {
            stack.Push((uint)stack.Pop() <= (uint)stack.Pop() ? 1 : 0);
        }
        
        public static void GREATER_EQUAL_LONG(this CScriptStack stack)
        {
            stack.Push(stack.Pop() <= stack.Pop() ? 1 : 0);
        }
        
        public static void GREATER_EQUAL_ULONG(this CScriptStack stack)
        {
            stack.Push((ulong)stack.Pop() <= (ulong)stack.Pop() ? 1 : 0);
        }
        
        public static unsafe void GREATER_EQUAL_FLOAT(this CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push(*(float*)&a <= *(float*)&b ? 1 : 0);
        }
        
        public static unsafe void GREATER_EQUAL_DOUBLE(this CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push(*(double*)&a <= *(double*)&b ? 1 : 0);
        }
        
        public static void LESS_BYTE(this CScriptStack stack)
        {
            stack.Push((byte)stack.Pop() > (byte)stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_SBYTE(this CScriptStack stack)
        {
            stack.Push((sbyte)stack.Pop() > (sbyte)stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_SHORT(this CScriptStack stack)
        {
            stack.Push((short)stack.Pop() > (short)stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_USHORT(this CScriptStack stack)
        {
            stack.Push((ushort)stack.Pop() > (ushort)stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_INT(this CScriptStack stack)
        {
            stack.Push((int)stack.Pop() > (int)stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_UINT(this CScriptStack stack)
        {
            stack.Push((uint)stack.Pop() > (uint)stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_LONG(this CScriptStack stack)
        {
            stack.Push(stack.Pop() > stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_ULONG(this CScriptStack stack)
        {
            stack.Push((ulong)stack.Pop() > (ulong)stack.Pop() ? 1 : 0);
        }
        
        public static unsafe void LESS_FLOAT(this CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push(*(float*)&a > *(float*)&b ? 1 : 0);
        }
        
        public static unsafe void LESS_DOUBLE(this CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push(*(double*)&a > *(double*)&b ? 1 : 0);
        }
        
        public static void LESS_EQUAL_BYTE(this CScriptStack stack)
        {
            stack.Push((byte)stack.Pop() >= (byte)stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_EQUAL_SBYTE(this CScriptStack stack)
        {
            stack.Push((sbyte)stack.Pop() >= (sbyte)stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_EQUAL_SHORT(this CScriptStack stack)
        {
            stack.Push((short)stack.Pop() >= (short)stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_EQUAL_USHORT(this CScriptStack stack)
        {
            stack.Push((ushort)stack.Pop() >= (ushort)stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_EQUAL_INT(this CScriptStack stack)
        {
            stack.Push((int)stack.Pop() >= (int)stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_EQUAL_UINT(this CScriptStack stack)
        {
            stack.Push((uint)stack.Pop() >= (uint)stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_EQUAL_LONG(this CScriptStack stack)
        {
            stack.Push(stack.Pop() >= stack.Pop() ? 1 : 0);
        }
        
        public static void LESS_EQUAL_ULONG(this CScriptStack stack)
        {
            stack.Push((ulong)stack.Pop() >= (ulong)stack.Pop() ? 1 : 0);
        }
        
        public static unsafe void LESS_EQUAL_FLOAT(this CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push(*(float*)&a >= *(float*)&b ? 1 : 0);
        }
        
        public static unsafe void LESS_EQUAL_DOUBLE(this CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push(*(double*)&a >= *(double*)&b ? 1 : 0);
        }
    }
}