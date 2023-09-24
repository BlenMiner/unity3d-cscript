using System.Runtime.CompilerServices;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ADD_CONSTANT(CScriptStack stack)
        {
            stack.STACK[stack.SP] += stack.Operand;
        }
        
        public static void ADD_BYTE_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (byte)((byte)stack.STACK[stack.SP] + (byte)stack.Operand);
        
        public static void ADD_SBYTE_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (sbyte)((sbyte)stack.STACK[stack.SP] + (sbyte)stack.Operand);
        
        public static void ADD_SHORT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (short)((short)stack.STACK[stack.SP] + (short)stack.Operand);
        
        public static void ADD_USHORT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (ushort)((ushort)stack.STACK[stack.SP] + (ushort)stack.Operand);
        
        public static void ADD_INT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (int)((int)stack.STACK[stack.SP] + (int)stack.Operand);
        
        public static void ADD_UINT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (uint)((uint)stack.STACK[stack.SP] + (uint)stack.Operand);
        
        public static void ADD_LONG_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (long)((long)stack.STACK[stack.SP] + (long)stack.Operand);
        
        public static void ADD_ULONG_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (long)((ulong)stack.STACK[stack.SP] + (ulong)stack.Operand);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ADD_FLOAT_CONSTANT(CScriptStack stack)
        {
            var a = stack.STACK[stack.SP];
            var res = (*(float*)&a + stack.Operand);
            stack.STACK[stack.SP] = *(long*)&res;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ADD_DOUBLE_CONSTANT(CScriptStack stack)
        {
            var a = stack.STACK[stack.SP];
            var res = (*(double*)&a + stack.Operand);
            stack.STACK[stack.SP] = *(long*)&res;
        }
        
        public static void SUB_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] -= stack.Operand;
        
        public static void SUB_BYTE_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (byte)((byte)stack.STACK[stack.SP] - (byte)stack.Operand);
        
        public static void SUB_SBYTE_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (sbyte)((sbyte)stack.STACK[stack.SP] - (sbyte)stack.Operand);
        
        public static void SUB_SHORT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (short)((short)stack.STACK[stack.SP] - (short)stack.Operand);
        
        public static void SUB_USHORT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (ushort)((ushort)stack.STACK[stack.SP] - (ushort)stack.Operand);
        
        public static void SUB_INT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (int)((int)stack.STACK[stack.SP] - (int)stack.Operand);
        
        public static void SUB_UINT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (uint)((uint)stack.STACK[stack.SP] - (uint)stack.Operand);
        
        public static void SUB_LONG_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (long)((long)stack.STACK[stack.SP] - (long)stack.Operand);
        
        public static void SUB_ULONG_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (long)((ulong)stack.STACK[stack.SP] - (ulong)stack.Operand);
        
        public static unsafe void SUB_FLOAT_CONSTANT(CScriptStack stack)
        {
            var a = stack.STACK[stack.SP];
            var res = (*(float*)&a - stack.Operand);
            stack.STACK[stack.SP] = *(long*)&res;
        }
        
        public static unsafe void SUB_DOUBLE_CONSTANT(CScriptStack stack)
        {
            var a = stack.STACK[stack.SP];
            var res = (*(double*)&a - stack.Operand);
            stack.STACK[stack.SP] = *(long*)&res;
        }
        
        public static void MUL_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] *= stack.Operand;
        
        public static void MUL_BYTE_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (byte)((byte)stack.STACK[stack.SP] * (byte)stack.Operand);
        
        public static void MUL_SBYTE_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (sbyte)((sbyte)stack.STACK[stack.SP] * (sbyte)stack.Operand);
        
        public static void MUL_SHORT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (short)((short)stack.STACK[stack.SP] * (short)stack.Operand);
        
        public static void MUL_USHORT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (ushort)((ushort)stack.STACK[stack.SP] * (ushort)stack.Operand);
        
        public static void MUL_INT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (int)((int)stack.STACK[stack.SP] * (int)stack.Operand);
        
        public static void MUL_UINT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (uint)((uint)stack.STACK[stack.SP] * (uint)stack.Operand);
        
        public static void MUL_LONG_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (long)((long)stack.STACK[stack.SP] * (long)stack.Operand);
        
        public static void MUL_ULONG_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (long)((ulong)stack.STACK[stack.SP] * (ulong)stack.Operand);
        
        public static unsafe void MUL_FLOAT_CONSTANT(CScriptStack stack)
        {
            var a = stack.STACK[stack.SP];
            var res = (*(float*)&a * stack.Operand);
            stack.STACK[stack.SP] = *(long*)&res;
        }
        
        public static unsafe void MUL_DOUBLE_CONSTANT(CScriptStack stack)
        {
            var a = stack.STACK[stack.SP];
            var res = (*(double*)&a * stack.Operand);
            stack.STACK[stack.SP] = *(long*)&res;
        }
        
        public static void DIV_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] /= stack.Operand;
        
        public static void DIV_BYTE_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (byte)((byte)stack.STACK[stack.SP] / (byte)stack.Operand);
        
        public static void DIV_SBYTE_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (sbyte)((sbyte)stack.STACK[stack.SP] / (sbyte)stack.Operand);
        
        public static void DIV_SHORT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (short)((short)stack.STACK[stack.SP] / (short)stack.Operand);
        
        public static void DIV_USHORT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (ushort)((ushort)stack.STACK[stack.SP] / (ushort)stack.Operand);
        
        public static void DIV_INT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (int)((int)stack.STACK[stack.SP] / (int)stack.Operand);
        
        public static void DIV_UINT_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (uint)((uint)stack.STACK[stack.SP] / (uint)stack.Operand);
        
        public static void DIV_LONG_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (long)((long)stack.STACK[stack.SP] / (long)stack.Operand);
        
        public static void DIV_ULONG_CONSTANT(CScriptStack stack) => stack.STACK[stack.SP] = (long)((ulong)stack.STACK[stack.SP] / (ulong)stack.Operand);
        
        public static unsafe void DIV_FLOAT_CONSTANT(CScriptStack stack)
        {
            var a = stack.STACK[stack.SP];
            var res = (*(float*)&a / stack.Operand);
            stack.STACK[stack.SP] = *(long*)&res;
        }
        
        public static unsafe void DIV_DOUBLE_CONSTANT(CScriptStack stack)
        {
            var a = stack.STACK[stack.SP];
            var res = (*(double*)&a / stack.Operand);
            stack.STACK[stack.SP] = *(long*)&res;
        }
    }
}