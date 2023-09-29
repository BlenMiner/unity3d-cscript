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
            var reg1 = property.FindPropertyRelative("Reg").intValue;
            var reg2 =property.FindPropertyRelative("Reg2").intValue;
            
            string content = opcode.ToString();
            
            if (reg1 != -1)
                content += " " + (Registers)reg1;
            
            if (reg2 != -1)
                content += ", " + (Registers)reg2;

            if (opcode is Opcodes.ADD or Opcodes.DUP or Opcodes.POP or Opcodes.REPEAT or Opcodes.REPEAT_END)
            {
                GUI.Label(position, content);
                return;
            }
            
            content += ", " + property.FindPropertyRelative("Operand").longValue;
            GUI.Label(position, content);
        }
    }
}