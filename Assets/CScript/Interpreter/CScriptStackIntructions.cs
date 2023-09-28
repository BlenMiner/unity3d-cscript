using UnityEngine;

namespace CScript
{
    public static unsafe class CScriptStackIntructions
    {
        public static void PRINT(CScriptStack stack)
        {
            var intVal = stack.Peek();
            Debug.Log("DEBUG, Top stack value: " + intVal);
        }
        
        public static void PRINT_FLOAT(CScriptStack stack)
        {
            var intVal = stack.Peek();
            float val = *(float*)&intVal;
            Debug.Log("DEBUG, Top stack value: " + val);
        }
    }
}