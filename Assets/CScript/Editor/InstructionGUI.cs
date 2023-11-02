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
            var operand3 = property.FindPropertyRelative("Operand3").intValue;
            
            string content = opcode.ToString();
            var opcodeName = opcode.ToString();
            
            bool isMath = opcodeName.StartsWith("ADD") || opcodeName.StartsWith("SUB") || opcodeName.StartsWith("MUL") || opcodeName.StartsWith("DIV");
            bool isCompare = opcodeName.StartsWith("LESS") || opcodeName.StartsWith("GREATER") || opcodeName.StartsWith("EQUAL");
            
            if (isMath || isCompare || opcode is Opcodes.REPEAT or Opcodes.REPEAT_END or Opcodes.REPEAT 
                or Opcodes.RETURN)
            {
                GUI.Label(position, content);
                return;
            }
            
            content += ", " + operand;

            switch (opcode)
            {
                case Opcodes.COPY_FROM_SPTR_TO_SPTR or Opcodes.SWAP_SPTR_SPTR or Opcodes.CALL_ARGS:
                    content += ", " + operand2;
                    break;
            }

            GUI.Label(position, content);
        }
    }
    
    [CustomPropertyDrawer(typeof(CTError))]
    public class ErrorGUI : PropertyDrawer
    {
        private GUIStyle m_skin;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            m_skin ??= new GUIStyle(GUI.skin.label)
            {
                richText = true
            };

            var error = property.FindPropertyRelative("Message").stringValue;
            EditorGUI.LabelField(position, error, m_skin);
        }
    }
}