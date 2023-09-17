using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CScriptInline))]
public class CScriptDynamicGUI : PropertyDrawer
{
    CScriptField m_field;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.managedReferenceValue == null)
        {
            var src = ScriptableObject.CreateInstance<CScript>();
            src.Setup(Random.Range(-100, 100).ToString());
            var inline = new CScriptInline(src);
            
            property.managedReferenceValue = inline;
        }
        
        var script = property.FindPropertyRelative("m_script").objectReferenceValue as CScript;

        if (!script)
            return base.GetPropertyHeight(property, label) + CScriptField.BOTTOM_MARGIN;

        return (base.GetPropertyHeight(property, label) - 3) * Mathf.Max(1, script.LineCount) + CScriptField.BOTTOM_MARGIN;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var focus = GUI.GetNameOfFocusedControl();
        EditorGUI.BeginProperty(position, label, property);
        
        m_field ??= new CScriptField(property.FindPropertyRelative("m_script").objectReferenceValue as CScript);
        m_field?.OnGUI(position);
        
        EditorGUI.EndProperty();
    }
}