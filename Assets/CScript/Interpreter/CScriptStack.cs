namespace CScript
{
    public sealed class CScriptStack
    {
        const int STACK_SIZE = 1024 * 1024;

        public readonly byte[] STACK = new byte[1024 * 1024];

        public int SP = STACK_SIZE - 1;
        
        public int StackByteSize => STACK_SIZE - SP - 1;
        
        public int IP;

        public long R0;
        public long R1;
        public long R2;
        public long R3;
        
        public long R4;
        public long R5;
        public long R6;
        public long R7;
        
        public unsafe void PushFloat(float value)
        {
            int* ptr = (int*)&value;
            PushInt(*ptr);
        }
        
        public unsafe void PushDouble(double value)
        {
            long* ptr = (long*)&value;
            PushLong(*ptr);
        }
        
        public void PushLong(long value)
        {
            int high = (int) (value >> 32);
            int low = (int) value;
            
            PushInt(low);
            PushInt(high);
        }
        
        public void PushULong(ulong value)
        {
            PushUInt((uint) value);
            PushUInt((uint) (value >> 32));
        }
        
        public void PushInt(long value)
        {
            STACK[--SP] = (byte) value;
            STACK[--SP] = (byte) (value >> 8);
            STACK[--SP] = (byte) (value >> 16);
            STACK[--SP] = (byte) (value >> 24);
        }
        
        public void PushUInt(ulong value)
        {
            STACK[--SP] = (byte) value;
            STACK[--SP] = (byte) (value >> 8);
            STACK[--SP] = (byte) (value >> 16);
            STACK[--SP] = (byte) (value >> 24);
        }
        
        public void PushShort(long value)
        {
            STACK[--SP] = (byte) value;
            STACK[--SP] = (byte) (value >> 8);
        }
        
        public void PushUShort(long value)
        {
            STACK[--SP] = (byte) value;
            STACK[--SP] = (byte) (value >> 8);
        }
        
        public void PushByte(byte value)
        {
            STACK[--SP] = value;
        }
        
        public unsafe void PushSByte(sbyte value)
        {
            STACK[--SP] = *(byte*)&value;
        }
        
        public byte PopByte()
        {
            return STACK[SP++];
        }
        
        public sbyte PopSByte()
        {
            return (sbyte) STACK[SP++];
        }
        
        public short PopShort()
        {
            return (short) (STACK[SP++] << 8 | STACK[SP++]);
        }
        
        public ushort PopUShort()
        {
            return (ushort) (STACK[SP++] << 8 | STACK[SP++]);
        }
        
        public int PopInt()
        {
            return (STACK[SP++] << 24) | 
                   (STACK[SP++] << 16) | 
                   (STACK[SP++] << 8) | 
                   STACK[SP++];
        }
        
        public uint PopUInt()
        {
            return (uint)PopInt();
        }
        
        public long PopLong()
        {
            long high = PopInt();
            uint low = (uint)PopInt();
            return (high << 32) | low;
        }

        public ulong PopULong()
        {
            uint high = PopUInt();
            uint low = PopUInt();
            return ((ulong)high << 32) | low;
        }
        
        public unsafe float PopFloat()
        {
            int value = PopInt();
            return *(float*)&value;
        }
        
        public unsafe double PopDouble()
        {
            long value = PopLong();
            return *(double*)&value;
        }
    }
}