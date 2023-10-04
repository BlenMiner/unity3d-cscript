using UnityEditor;
using Riten.CScript.Native;
using UnityEngine;

namespace Riten.CScript.Editor
{
    [CustomPropertyDrawer(typeof(Instruction))]
    public class InstructionGUI : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var opcode = (Opcodes)property.FindPropertyRelative("Opcode").intValue;
            var operand = property.FindPropertyRelative("Operand").longValue;
            var operand2 = property.FindPropertyRelative("Operand2").intValue;
            
            string content = opcode.ToString();
            
            if (opcode is Opcodes.ADD or Opcodes.DUP or Opcodes.POP or Opcodes.REPEAT or Opcodes.REPEAT_END or Opcodes.REPEAT 
                or Opcodes.RETURN)
            {
                GUI.Label(position, content);
                return;
            }
            
            content += ", " + operand;

            if (opcode is Opcodes.PUSH_CONST_TO_SPTR or Opcodes.COPY_FROM_SPTR_TO_SPTR)
            {
                content += ", " + operand2;
            }
            
            GUI.Label(position, content);
        }
    }
}