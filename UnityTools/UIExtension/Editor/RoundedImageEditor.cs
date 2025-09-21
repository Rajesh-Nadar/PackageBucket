using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace UnityTools.UIExtension
{
    [CustomEditor(typeof(RoundedImage))]
    public class RoundedImageEditor : ImageEditor
    {
        SerializedProperty cornerRadius;
        SerializedProperty cornerSegments;

        protected override void OnEnable()
        {
            base.OnEnable();
            cornerRadius = serializedObject.FindProperty("cornerRadius");
            cornerSegments = serializedObject.FindProperty("cornerSegments");
        }

        public override void OnInspectorGUI()
        {
            // Draw the default Image inspector
            base.OnInspectorGUI();

            // Add a separator
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rounded Rectangle Settings", EditorStyles.boldLabel);

            // Show custom properties
            serializedObject.Update();
            EditorGUILayout.PropertyField(cornerRadius);
            EditorGUILayout.PropertyField(cornerSegments);
            serializedObject.ApplyModifiedProperties();
        }
    }
}