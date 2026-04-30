using UnityEditor;
using UnityEngine;

public class LabelAttribute : PropertyAttribute
{
    public string NewName { get; private set; }
    public int MinLines { get; private set; }
    public int MaxLines { get; private set; }
    public bool IsTextArea { get; private set; }

    public LabelAttribute(string name)
    {
        NewName = name;
        IsTextArea = false;
    }

    // TextArea 기능을 포함하는 생성자 오버로딩
    public LabelAttribute(string name, int minLines, int maxLines)
    {
        NewName = name;
        MinLines = minLines;
        MaxLines = maxLines;
        IsTextArea = true;
    }
}

[CustomPropertyDrawer(typeof(LabelAttribute))]
public class RenameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        LabelAttribute rename = (LabelAttribute)attribute;
        label.text = rename.NewName; 
        
        if (property.propertyType == SerializedPropertyType.String && rename.IsTextArea)
        {
            Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, label);

            Rect textAreaRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height - EditorGUIUtility.singleLineHeight);
            property.stringValue = EditorGUI.TextArea(textAreaRect, property.stringValue);
        }
        else
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        LabelAttribute rename = (LabelAttribute)attribute;
        
        if (property.propertyType == SerializedPropertyType.String && rename.IsTextArea)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            return (lineHeight * rename.MinLines) + lineHeight + 5f; 
        }

        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}