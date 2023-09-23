namespace CScript
{
    public static class CScriptMathInstructions
    {
        public static void ADD_BYTE(CScriptStack stack)
        {
            var a = stack.PopByte();
            var b = stack.PopByte();
            stack.PushByte((byte)(a + b));
        }

        public static void ADD_SBYTE(CScriptStack stack)
        {
            var a = stack.PopSByte();
            var b = stack.PopSByte();
            stack.PushSByte((sbyte)(a + b));
        }

        public static void ADD_SHORT(CScriptStack stack)
        {
            var a = stack.PopShort();
            var b = stack.PopShort();
            stack.PushShort((short)(a + b));
        }

        public static void ADD_USHORT(CScriptStack stack)
        {
            var a = stack.PopUShort();
            var b = stack.PopUShort();
            stack.PushUShort((ushort)(a + b));
        }

        public static void ADD_INT(CScriptStack stack)
        {
            var a = stack.PopInt();
            var b = stack.PopInt();
            
            stack.PushInt(a + b);
        }

        public static void ADD_UINT(CScriptStack stack)
        {
            var a = stack.PopUInt();
            var b = stack.PopUInt();
            stack.PushUInt(a + b);
        }

        public static void ADD_LONG(CScriptStack stack)
        {
            var a = stack.PopLong();
            var b = stack.PopLong();
            stack.PushLong(a + b);
        }

        public static void ADD_ULONG(CScriptStack stack)
        {
            var a = stack.PopULong();
            var b = stack.PopULong();
            stack.PushULong(a + b);
        }

        public static void ADD_FLOAT(CScriptStack stack)
        {
            var a = stack.PopFloat();
            var b = stack.PopFloat();
            stack.PushFloat(a + b);
        }

        public static void ADD_DOUBLE(CScriptStack stack)
        {
            var a = stack.PopDouble();
            var b = stack.PopDouble();
            stack.PushDouble(a + b);
        }

        public static void SUB_BYTE(CScriptStack stack)
        {
            var a = stack.PopByte();
            var b = stack.PopByte();
            stack.PushByte((byte)(a - b));
        }

        public static void SUB_SBYTE(CScriptStack stack)
        {
            var a = stack.PopSByte();
            var b = stack.PopSByte();
            stack.PushSByte((sbyte)(a - b));
        }

        public static void SUB_SHORT(CScriptStack stack)
        {
            var a = stack.PopShort();
            var b = stack.PopShort();
            stack.PushShort((short)(a - b));
        }

        public static void SUB_USHORT(CScriptStack stack)
        {
            var a = stack.PopUShort();
            var b = stack.PopUShort();
            stack.PushUShort((ushort)(a - b));
        }

        public static void SUB_INT(CScriptStack stack)
        {
            var a = stack.PopInt();
            var b = stack.PopInt();
            stack.PushInt(a - b);
        }

        public static void SUB_UINT(CScriptStack stack)
        {
            var a = stack.PopUInt();
            var b = stack.PopUInt();
            stack.PushUInt(a - b);
        }

        public static void SUB_LONG(CScriptStack stack)
        {
            var a = stack.PopLong();
            var b = stack.PopLong();
            stack.PushLong(a - b);
        }

        public static void SUB_ULONG(CScriptStack stack)
        {
            var a = stack.PopULong();
            var b = stack.PopULong();
            stack.PushULong(a - b);
        }

        public static void SUB_FLOAT(CScriptStack stack)
        {
            var a = stack.PopFloat();
            var b = stack.PopFloat();
            stack.PushFloat(a - b);
        }

        public static void SUB_DOUBLE(CScriptStack stack)
        {
            var a = stack.PopDouble();
            var b = stack.PopDouble();
            stack.PushDouble(a - b);
        }

        public static void MUL_BYTE(CScriptStack stack)
        {
            var a = stack.PopByte();
            var b = stack.PopByte();
            stack.PushByte((byte)(a * b));
        }

        public static void MUL_SBYTE(CScriptStack stack)
        {
            var a = stack.PopSByte();
            var b = stack.PopSByte();
            stack.PushSByte((sbyte)(a * b));
        }

        public static void MUL_SHORT(CScriptStack stack)
        {
            var a = stack.PopShort();
            var b = stack.PopShort();
            stack.PushShort((short)(a * b));
        }

        public static void MUL_USHORT(CScriptStack stack)
        {
            var a = stack.PopUShort();
            var b = stack.PopUShort();
            stack.PushUShort((ushort)(a * b));
        }

        public static void MUL_INT(CScriptStack stack)
        {
            var a = stack.PopInt();
            var b = stack.PopInt();
            stack.PushInt(a * b);
        }

        public static void MUL_UINT(CScriptStack stack)
        {
            var a = stack.PopUInt();
            var b = stack.PopUInt();
            stack.PushUInt(a * b);
        }

        public static void MUL_LONG(CScriptStack stack)
        {
            var a = stack.PopLong();
            var b = stack.PopLong();
            stack.PushLong(a * b);
        }

        public static void MUL_ULONG(CScriptStack stack)
        {
            var a = stack.PopULong();
            var b = stack.PopULong();
            stack.PushULong(a * b);
        }

        public static void MUL_FLOAT(CScriptStack stack)
        {
            var a = stack.PopFloat();
            var b = stack.PopFloat();
            stack.PushFloat(a * b);
        }

        public static void MUL_DOUBLE(CScriptStack stack)
        {
            var a = stack.PopDouble();
            var b = stack.PopDouble();
            stack.PushDouble(a * b);
        }

        public static void DIV_BYTE(CScriptStack stack)
        {
            var a = stack.PopByte();
            var b = stack.PopByte();
            stack.PushByte((byte)(a / b));
        }

        public static void DIV_SBYTE(CScriptStack stack)
        {
            var a = stack.PopSByte();
            var b = stack.PopSByte();
            stack.PushSByte((sbyte)(a / b));
        }

        public static void DIV_SHORT(CScriptStack stack)
        {
            var a = stack.PopShort();
            var b = stack.PopShort();
            stack.PushShort((short)(a / b));
        }

        public static void DIV_USHORT(CScriptStack stack)
        {
            var a = stack.PopUShort();
            var b = stack.PopUShort();
            stack.PushUShort((ushort)(a / b));
        }

        public static void DIV_INT(CScriptStack stack)
        {
            var a = stack.PopInt();
            var b = stack.PopInt();
            stack.PushInt(a / b);
        }

        public static void DIV_UINT(CScriptStack stack)
        {
            var a = stack.PopUInt();
            var b = stack.PopUInt();
            stack.PushUInt(a / b);
        }

        public static void DIV_LONG(CScriptStack stack)
        {
            var a = stack.PopLong();
            var b = stack.PopLong();
            stack.PushLong(a / b);
        }

        public static void DIV_ULONG(CScriptStack stack)
        {
            var a = stack.PopULong();
            var b = stack.PopULong();
            stack.PushULong(a / b);
        }

        public static void DIV_FLOAT(CScriptStack stack)
        {
            var a = stack.PopFloat();
            var b = stack.PopFloat();
            stack.PushFloat(a / b);
        }

        public static void DIV_DOUBLE(CScriptStack stack)
        {
            var a = stack.PopDouble();
            var b = stack.PopDouble();
            stack.PushDouble(a / b);
        }
    }
}