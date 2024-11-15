using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.AdvancedAttributes
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Disable GUI to make the field read-only
            GUI.enabled = false;

            // Draw the property as usual
            EditorGUI.PropertyField(position, property, label);

            // Re-enable GUI for other fields
            GUI.enabled = true;
        }
    }
}