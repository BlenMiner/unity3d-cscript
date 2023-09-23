using System.Runtime.CompilerServices;
using UnityEngine.Profiling;

namespace CScript
{
    public sealed class CScriptStack
    {
        const int STACK_SIZE = 512 * 512;

        public readonly long[] STACK = new long[STACK_SIZE];

        public int SP = STACK_SIZE - 1;
        
        public int StackByteSize => STACK_SIZE - SP - 1;
        
        public int IP;
        public long Operand;
        
        public readonly long[] REGISTERS = new long[8];
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(long value)
        {
            STACK[--SP] = value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Pop()
        {
            return STACK[SP++];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Peek()
        {
            return STACK[SP];
        }
    }
}