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
            src.Setup(CScriptDynamic.DEFAULT_CODE);
            var inline = new CScriptInline(src);
            
            property.managedReferenceValue = inline;
        }
        
        var script = property.FindPropertyRelative("m_script").objectReferenceValue as CScript;
        
        if (!script)
            return base.GetPropertyHeight(property, label);

        return (base.GetPropertyHeight(property, label) - 3) * Mathf.Max(1, script.LineCount) + 
               (CScriptField.BOTTOM_MARGIN * (script.RootNode != null && script.RootNode.Errors.Count > 0 ? 1 : 0));
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        m_field ??= new CScriptField(property.FindPropertyRelative("m_script").objectReferenceValue as CScript);
        
        EditorGUI.LabelField(position, label);
        
        float labelWidth = EditorGUIUtility.labelWidth * 0.5f;
        
        position.x += labelWidth;
        position.width -= labelWidth;
        
        if (m_field.OnGUI(position))
            EditorUtility.SetDirty(property.serializedObject.targetObject);

        EditorGUI.EndProperty();
    }
}