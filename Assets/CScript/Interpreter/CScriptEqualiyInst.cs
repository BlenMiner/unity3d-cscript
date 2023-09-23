namespace CScript
{
    public static class CScriptEqualiyInst
    {
        public static void EQUAL_BYTE(this CScriptStack stack)
        {
            byte res = stack.PopByte() == stack.PopByte() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }
        
        public static void EQUAL_SBYTE(this CScriptStack stack)
        {
            byte res = stack.PopSByte() == stack.PopSByte() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }
        
        public static void EQUAL_SHORT(this CScriptStack stack)
        {
            byte res = stack.PopShort() == stack.PopShort() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }
        
        public static void EQUAL_USHORT(this CScriptStack stack)
        {
            byte res = stack.PopUShort() == stack.PopUShort() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }
        
        public static void EQUAL_INT(this CScriptStack stack)
        {
            byte res = stack.PopInt() == stack.PopInt() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }
        
        public static void EQUAL_UINT(this CScriptStack stack)
        {
            byte res = stack.PopUInt() == stack.PopUInt() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }
        
        public static void EQUAL_LONG(this CScriptStack stack)
        {
            byte res = stack.PopLong() == stack.PopLong() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }
        
        public static void EQUAL_ULONG(this CScriptStack stack)
        {
            byte res = stack.PopULong() == stack.PopULong() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }
        
        public static void EQUAL_FLOAT(this CScriptStack stack)
        {
            byte res = stack.PopFloat() == stack.PopFloat() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }
        
        public static void EQUAL_DOUBLE(this CScriptStack stack)
        {
            byte res = stack.PopDouble() == stack.PopDouble() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }
        
        public static void NOT_EQUAL_BYTE(this CScriptStack stack)
        {
            byte res = stack.PopByte() != stack.PopByte() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void NOT_EQUAL_SBYTE(this CScriptStack stack)
        {
            byte res = stack.PopSByte() != stack.PopSByte() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void NOT_EQUAL_SHORT(this CScriptStack stack)
        {
            byte res = stack.PopShort() != stack.PopShort() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void NOT_EQUAL_USHORT(this CScriptStack stack)
        {
            byte res = stack.PopUShort() != stack.PopUShort() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void NOT_EQUAL_INT(this CScriptStack stack)
        {
            byte res = stack.PopInt() != stack.PopInt() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void NOT_EQUAL_UINT(this CScriptStack stack)
        {
            byte res = stack.PopUInt() != stack.PopUInt() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void NOT_EQUAL_LONG(this CScriptStack stack)
        {
            byte res = stack.PopLong() != stack.PopLong() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void NOT_EQUAL_ULONG(this CScriptStack stack)
        {
            byte res = stack.PopULong() != stack.PopULong() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void NOT_EQUAL_FLOAT(this CScriptStack stack)
        {
            byte res = stack.PopFloat() != stack.PopFloat() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void NOT_EQUAL_DOUBLE(this CScriptStack stack)
        {
            byte res = stack.PopDouble() != stack.PopDouble() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }
        
        public static void GREATER_THAN_BYTE(this CScriptStack stack)
        {
            byte res = stack.PopByte() > stack.PopByte() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_THAN_SBYTE(this CScriptStack stack)
        {
            byte res = stack.PopSByte() > stack.PopSByte() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_THAN_SHORT(this CScriptStack stack)
        {
            byte res = stack.PopShort() > stack.PopShort() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_THAN_USHORT(this CScriptStack stack)
        {
            byte res = stack.PopUShort() > stack.PopUShort() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_THAN_INT(this CScriptStack stack)
        {
            byte res = stack.PopInt() > stack.PopInt() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_THAN_UINT(this CScriptStack stack)
        {
            byte res = stack.PopUInt() > stack.PopUInt() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_THAN_LONG(this CScriptStack stack)
        {
            byte res = stack.PopLong() > stack.PopLong() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_THAN_ULONG(this CScriptStack stack)
        {
            byte res = stack.PopULong() > stack.PopULong() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_THAN_FLOAT(this CScriptStack stack)
        {
            byte res = stack.PopFloat() > stack.PopFloat() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_THAN_DOUBLE(this CScriptStack stack)
        {
            byte res = stack.PopDouble() > stack.PopDouble() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_EQUAL_BYTE(this CScriptStack stack)
        {
            byte res = stack.PopByte() >= stack.PopByte() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_EQUAL_SBYTE(this CScriptStack stack)
        {
            byte res = stack.PopSByte() >= stack.PopSByte() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_EQUAL_SHORT(this CScriptStack stack)
        {
            byte res = stack.PopShort() >= stack.PopShort() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_EQUAL_USHORT(this CScriptStack stack)
        {
            byte res = stack.PopUShort() >= stack.PopUShort() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_EQUAL_INT(this CScriptStack stack)
        {
            byte res = stack.PopInt() >= stack.PopInt() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_EQUAL_UINT(this CScriptStack stack)
        {
            byte res = stack.PopUInt() >= stack.PopUInt() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_EQUAL_LONG(this CScriptStack stack)
        {
            byte res = stack.PopLong() >= stack.PopLong() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_EQUAL_ULONG(this CScriptStack stack)
        {
            byte res = stack.PopULong() >= stack.PopULong() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_EQUAL_FLOAT(this CScriptStack stack)
        {
            byte res = stack.PopFloat() >= stack.PopFloat() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void GREATER_EQUAL_DOUBLE(this CScriptStack stack)
        {
            byte res = stack.PopDouble() >= stack.PopDouble() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_BYTE(this CScriptStack stack)
        {
            byte res = stack.PopByte() < stack.PopByte() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_SBYTE(this CScriptStack stack)
        {
            byte res = stack.PopSByte() < stack.PopSByte() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_SHORT(this CScriptStack stack)
        {
            byte res = stack.PopShort() < stack.PopShort() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_USHORT(this CScriptStack stack)
        {
            byte res = stack.PopUShort() < stack.PopUShort() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_INT(this CScriptStack stack)
        {
            byte res = stack.PopInt() < stack.PopInt() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_UINT(this CScriptStack stack)
        {
            byte res = stack.PopUInt() < stack.PopUInt() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_LONG(this CScriptStack stack)
        {
            byte res = stack.PopLong() < stack.PopLong() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_ULONG(this CScriptStack stack)
        {
            byte res = stack.PopULong() < stack.PopULong() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_FLOAT(this CScriptStack stack)
        {
            byte res = stack.PopFloat() < stack.PopFloat() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_DOUBLE(this CScriptStack stack)
        {
            byte res = stack.PopDouble() < stack.PopDouble() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_EQUAL_BYTE(this CScriptStack stack)
        {
            byte res = stack.PopByte() <= stack.PopByte() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_EQUAL_SBYTE(this CScriptStack stack)
        {
            byte res = stack.PopSByte() <= stack.PopSByte() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_EQUAL_SHORT(this CScriptStack stack)
        {
            byte res = stack.PopShort() <= stack.PopShort() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_EQUAL_USHORT(this CScriptStack stack)
        {
            byte res = stack.PopUShort() <= stack.PopUShort() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_EQUAL_INT(this CScriptStack stack)
        {
            byte res = stack.PopInt() <= stack.PopInt() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_EQUAL_UINT(this CScriptStack stack)
        {
            byte res = stack.PopUInt() <= stack.PopUInt() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_EQUAL_LONG(this CScriptStack stack)
        {
            byte res = stack.PopLong() <= stack.PopLong() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_EQUAL_ULONG(this CScriptStack stack)
        {
            byte res = stack.PopULong() <= stack.PopULong() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_EQUAL_FLOAT(this CScriptStack stack)
        {
            byte res = stack.PopFloat() <= stack.PopFloat() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

        public static void LESS_EQUAL_DOUBLE(this CScriptStack stack)
        {
            byte res = stack.PopDouble() <= stack.PopDouble() ? (byte)1 : (byte)0;
            stack.PushByte(res);
        }

    }
}