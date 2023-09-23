using UnityEngine;

namespace CScript
{
    public static class CScriptStackIntructions
    {
        public static void DEBUG(this CScriptStack stack)
        {
            var intVal = stack.Peek();
            Debug.Log("DEBUG, Int top stack: " + intVal);
        }
        
        public static void PushLong(this CScriptStack stack)
        {
            stack.STACK[--stack.SP] = stack.Operand;
        }
        
        public static void PopLong(this CScriptStack stack)
        {
            stack.Operand = stack.STACK[stack.SP++];
        }
    }
}