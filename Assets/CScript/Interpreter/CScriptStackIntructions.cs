using System.Runtime.CompilerServices;
using UnityEngine;

namespace CScript
{
    public static unsafe class CScriptStackIntructions
    {
        public static void DEBUG(this CScriptStack stack)
        {
            var intVal = stack.Peek();
            Debug.Log("DEBUG, Int top stack: " + intVal);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushLong(this CScriptStack stack)
        {
            fixed (long* ptr = stack.STACK)
            {
                *(ptr + --stack.SP) = stack.Operand;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PopLong(this CScriptStack stack)
        {
            fixed (long* ptr = stack.STACK)
            {
                stack.Operand = *(ptr + stack.SP++);
            }
        }
    }
}