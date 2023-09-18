using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CScriptUID))]
public class CReadOnlyEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        GUI.enabled = false;
        GUI.Label(position, property.FindPropertyRelative("Value").stringValue);
        GUI.enabled = true;
        
        EditorGUI.EndProperty();
    }
}
