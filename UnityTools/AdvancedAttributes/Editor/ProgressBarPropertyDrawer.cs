using UnityEditor;
using UnityEngine;

namespace UnityTools.AdvancedAttributes
{
    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarPropertyDrawer : PropertyDrawer
    {
        // Override the OnGUI method to customize the property display
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Ensure the property is an integer type
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                // Get the ProgressBarAttribute attached to the property
                ProgressBarAttribute progressBar = (ProgressBarAttribute) attribute;

                // Get the current value of the integer field
                int currentValue = property.intValue;

                // Calculate the percentage of the progress bar
                float progress = Mathf.Clamp01(currentValue / progressBar.maxValue);

                // Save the initial position to handle the height calculation
                Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                Rect progressBarRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, 10); // Adjusted height for progress bar
                Rect intFieldRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 14, position.width, EditorGUIUtility.singleLineHeight); // Integer field below the bar

                // Draw the label
                EditorGUI.LabelField(labelRect, label);

                // Draw the progress bar background
                EditorGUI.DrawRect(progressBarRect, Color.gray);

                // Draw the progress bar (foreground)
                Rect progressRect = new Rect(progressBarRect.x, progressBarRect.y, progressBarRect.width * progress, progressBarRect.height);
                EditorGUI.DrawRect(progressRect, Color.green);

                // Draw the integer field below the progress bar
                property.intValue = EditorGUI.IntField(intFieldRect, "Value", currentValue);

                // Set the height of the entire property for proper layout
                float totalHeight = EditorGUIUtility.singleLineHeight + 14 + EditorGUIUtility.singleLineHeight;
                position.height = totalHeight;
            }
            else
            {
                // Draw the default field if the property is not an integer
                EditorGUI.PropertyField(position, property, label);
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {

            return  EditorGUIUtility.singleLineHeight * 3;
        }
    }
}