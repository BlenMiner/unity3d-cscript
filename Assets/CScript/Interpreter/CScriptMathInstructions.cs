namespace CScript
{
    public static class CScriptMathInstructions
    {
        public static void ADD_BYTE(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push((byte)(a + b));
        }

        public static void ADD_SBYTE(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push((sbyte)(a + b));
        }

        public static void ADD_SHORT(CScriptStack stack)
        {
            var a = (short)stack.Pop();
            var b = (short)stack.Pop();
            stack.Push((short)(a + b));
        }

        public static void ADD_USHORT(CScriptStack stack)
        {
            var a = (ushort)stack.Pop();
            var b = (ushort)stack.Pop();
            stack.Push((ushort)(a + b));
        }

        public static void ADD_INT(CScriptStack stack)
        {
            var a = (int)stack.Pop();
            var b = (int)stack.Pop();
            
            stack.Push(a + b);
        }

        public static void ADD_UINT(CScriptStack stack)
        {
            var a = (uint)stack.Pop();
            var b = (uint)stack.Pop();
            stack.Push(a + b);
        }

        public static void ADD_LONG(CScriptStack stack)
        {
            var a = stack.STACK[stack.SP++];
            stack.STACK[stack.SP] += a;
        }

        public static void ADD_ULONG(CScriptStack stack)
        {
            var a = (ulong)stack.Pop();
            var b = (ulong)stack.Pop();
            stack.Push((long)(a + b));
        }

        public static unsafe void ADD_FLOAT(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            var res = (*(float*)&a + *(float*)&b);
            stack.Push(*(long*)&res);
        }

        public static unsafe void ADD_DOUBLE(CScriptStack stack)
        {
            var a = stack.STACK[stack.SP++];
            var b = stack.STACK[stack.SP++];
            var res = (*(double*)&a + *(double*)&b);
            stack.Push(*(long*)&res);
        }

        public static void SUB_BYTE(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push((byte)(b - a));
        }
        
        public static void SUB_SBYTE(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push((sbyte)(b - a));
        }
        
        public static void SUB_SHORT(CScriptStack stack)
        {
            var a = (short)stack.Pop();
            var b = (short)stack.Pop();
            stack.Push((short)(b - a));
        }
        
        public static void SUB_USHORT(CScriptStack stack)
        {
            var a = (ushort)stack.Pop();
            var b = (ushort)stack.Pop();
            stack.Push((ushort)(b - a));
        }
        
        public static void SUB_INT(CScriptStack stack)
        {
            var a = (int)stack.Pop();
            var b = (int)stack.Pop();
            stack.Push(b - a);
        }
        
        public static void SUB_UINT(CScriptStack stack)
        {
            var a = (uint)stack.Pop();
            var b = (uint)stack.Pop();
            stack.Push(b - a);
        }
        
        public static void SUB_LONG(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push(b - a);
        }
        
        public static void SUB_ULONG(CScriptStack stack)
        {
            var a = (ulong)stack.Pop();
            var b = (ulong)stack.Pop();
            stack.Push((long)(b - a));
        }
        
        public static unsafe void SUB_FLOAT(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            var res = (*(float*)&b - *(float*)&a);
            stack.Push(*(long*)&res);
        }
        
        public static unsafe void SUB_DOUBLE(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            var res = (*(double*)&b - *(double*)&a);
            stack.Push(*(long*)&res);
        }
        
        public static void MUL_BYTE(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push((byte)(a * b));
        }
        
        public static void MUL_SBYTE(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push((sbyte)(a * b));
        }
        
        public static void MUL_SHORT(CScriptStack stack)
        {
            var a = (short)stack.Pop();
            var b = (short)stack.Pop();
            stack.Push((short)(a * b));
        }
        
        public static void MUL_USHORT(CScriptStack stack)
        {
            var a = (ushort)stack.Pop();
            var b = (ushort)stack.Pop();
            stack.Push((ushort)(a * b));
        }
        
        public static void MUL_INT(CScriptStack stack)
        {
            var a = (int)stack.Pop();
            var b = (int)stack.Pop();
            stack.Push(a * b);
        }
        
        public static void MUL_UINT(CScriptStack stack)
        {
            var a = (uint)stack.Pop();
            var b = (uint)stack.Pop();
            stack.Push(a * b);
        }
        
        public static void MUL_LONG(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push(a * b);
        }
        
        public static void MUL_ULONG(CScriptStack stack)
        {
            var a = (ulong)stack.Pop();
            var b = (ulong)stack.Pop();
            stack.Push((long)(a * b));
        }
        
        public static unsafe void MUL_FLOAT(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            var res = (*(float*)&a * *(float*)&b);
            stack.Push(*(long*)&res);
        }
        
        public static unsafe void MUL_DOUBLE(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            var res = (*(double*)&a * *(double*)&b);
            stack.Push(*(long*)&res);
        }
        
        public static void DIV_BYTE(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push((byte)(b / a));
        }
        
        public static void DIV_SBYTE(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push((sbyte)(b / a));
        }
        
        public static void DIV_SHORT(CScriptStack stack)
        {
            var a = (short)stack.Pop();
            var b = (short)stack.Pop();
            stack.Push((short)(b / a));
        }
        
        public static void DIV_USHORT(CScriptStack stack)
        {
            var a = (ushort)stack.Pop();
            var b = (ushort)stack.Pop();
            stack.Push((ushort)(b / a));
        }
        
        public static void DIV_INT(CScriptStack stack)
        {
            var a = (int)stack.Pop();
            var b = (int)stack.Pop();
            stack.Push(b / a);
        }
        
        public static void DIV_UINT(CScriptStack stack)
        {
            var a = (uint)stack.Pop();
            var b = (uint)stack.Pop();
            stack.Push(b / a);
        }
        
        public static void DIV_LONG(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            stack.Push(b / a);
        }
        
        public static void DIV_ULONG(CScriptStack stack)
        {
            var a = (ulong)stack.Pop();
            var b = (ulong)stack.Pop();
            stack.Push((long)(b / a));
        }
        
        public static unsafe void DIV_FLOAT(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            var res = (*(float*)&b / *(float*)&a);
            stack.Push(*(long*)&res);
        }
        
        public static unsafe void DIV_DOUBLE(CScriptStack stack)
        {
            var a = stack.Pop();
            var b = stack.Pop();
            var res = (*(double*)&b / *(double*)&a);
            stack.Push(*(long*)&res);
        }
    }
}