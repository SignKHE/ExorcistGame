using UnityEditor;
using UnityEngine;

public class LabelAttribute : PropertyAttribute
{
    public string NewName { get; private set; }
    public LabelAttribute(string name)
    {
        NewName = name;
    }
}

[CustomPropertyDrawer(typeof(LabelAttribute))]
public class RenameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        LabelAttribute rename = (LabelAttribute)attribute;
        label.text = rename.NewName; 
        
        EditorGUI.PropertyField(position, property, label);
    }
}