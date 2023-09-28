using System;
using System.Runtime.CompilerServices;

namespace CScript
{
    public sealed class CScriptStack
    {
        public const int STACK_SIZE = 512 * 512;

        public readonly long[] STACK_RAW = new long[STACK_SIZE];
        public readonly Memory<long> STACK_MEMORY;

        public int SP = STACK_SIZE - 1;
        
        public int StackByteSize => (STACK_SIZE - SP - 1) * 8;
        
        public readonly long[] REGISTERS = new long[8];

        public CScriptStack()
        {
            STACK_MEMORY = new Memory<long>(STACK_RAW);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(long value)
        {
            STACK_MEMORY.Span[--SP] = value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Pop()
        {
            return STACK_MEMORY.Span[SP++];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Peek()
        {
            return STACK_MEMORY.Span[SP];
        }
    }
}