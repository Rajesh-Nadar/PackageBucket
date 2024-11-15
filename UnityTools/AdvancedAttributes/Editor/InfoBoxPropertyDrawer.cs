using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.AdvancedAttributes
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(InfoBoxAttribute))]
    public class InfoBoxPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get attribute values
            InfoBoxAttribute info = attribute as InfoBoxAttribute;

            // Calculate help box height.
            float helpBoxHeight = GetHelpBoxHeight(info);

            // Initialize position of help box.
            var helpBoxRect = new Rect(position.x, position.y, position.width, helpBoxHeight);

            // Initialize position of property.
            var propertyFieldRect = new Rect(position.x, position.y + helpBoxHeight + 3, position.width, EditorGUIUtility.singleLineHeight);

            // Draw the help box.
            EditorGUI.HelpBox(helpBoxRect, info.message, info.type);

            // Draw the property as usual
            EditorGUI.PropertyField(propertyFieldRect, property, label);
        }

        private float GetHelpBoxHeight(InfoBoxAttribute info)
        {
            var lines = info.message.Split('\n');
            var height = (14 * lines.Length);

            if(height < 35)
                height = 35;

            return height;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            InfoBoxAttribute info = attribute as InfoBoxAttribute;
            float helpBoxHeight = GetHelpBoxHeight(info);

            return helpBoxHeight + EditorGUIUtility.singleLineHeight + 3;
        }
    }
}