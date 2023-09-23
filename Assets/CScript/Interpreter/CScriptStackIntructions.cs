using UnityEngine;

namespace CScript
{
    public static class CScriptStackIntructions
    {
        public static void PushByte(this CScriptStack stack)
        {
            stack.PushByte((byte)stack.R0);
        }
        
        public static void PushSByte(this CScriptStack stack)
        {
            stack.PushSByte((sbyte)stack.R0);
        }
        
        public static void PushShort(this CScriptStack stack)
        {
            stack.PushShort(stack.R0);
        }
        
        public static void PushUShort(this CScriptStack stack)
        {
            stack.PushUShort(stack.R0);
        }
        
        public static void PushInt(this CScriptStack stack)
        {
            stack.PushInt((int)stack.R0);
        }
        
        public static void PushUInt(this CScriptStack stack)
        {
            stack.PushUInt((uint)stack.R0);
        }
        
        public static void PushLong(this CScriptStack stack)
        {
            stack.PushLong(stack.R0);
        }
        
        public static void PushULong(this CScriptStack stack)
        {
            stack.PushULong((ulong)stack.R0);
        }
        
        public static unsafe void PushFloat(this CScriptStack stack)
        {
            var v = stack.R0;
            stack.PushFloat(*(float*)&v);
        }
        
        public static unsafe void PushDouble(this CScriptStack stack)
        {
            var v = stack.R0;
            stack.PushDouble(*(double*)&v);
        }
        
        public static void PopByte(this CScriptStack stack)
        {
            stack.R0 = stack.PopByte();
        }
        
        public static void PopSByte(this CScriptStack stack)
        {
            stack.R0 = stack.PopSByte();
        }
        
        public static void PopShort(this CScriptStack stack)
        {
            stack.R0 = stack.PopShort();
        }
        
        public static void PopUShort(this CScriptStack stack)
        {
            stack.R0 = stack.PopUShort();
        }
        
        public static void PopInt(this CScriptStack stack)
        {
            stack.R0 = stack.PopInt();
        }
        
        public static void PopUInt(this CScriptStack stack)
        {
            stack.R0 = stack.PopUInt();
        }
        
        public static void PopLong(this CScriptStack stack)
        {
            stack.R0 = stack.PopLong();
        }
        
        public static void PopULong(this CScriptStack stack)
        {
            stack.R0 = (long)stack.PopULong();
        }
        
        public static void PopFloat(this CScriptStack stack)
        {
            stack.R0 = stack.PopInt();
        }
        
        public static void PopDouble(this CScriptStack stack)
        {
            stack.R0 = stack.PopLong();
        }
    }
}