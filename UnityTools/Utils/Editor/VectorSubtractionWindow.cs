
namespace UnityTools.Utils
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class VectorMathWindow : EditorWindow
    {
        private Vector3 vectorA = Vector3.zero;
        private Vector3 vectorB = Vector3.zero;
        private Vector3 result = Vector3.zero;

        private Vector2 scrollPos;

        // Struct to hold full history info
        private struct HistoryEntry
        {
            public Vector3 A;
            public Vector3 B;
            public Vector3 Result;
            public string Operation; // "A - B" or "A + B"

            public HistoryEntry(Vector3 a, Vector3 b, Vector3 result, string operation)
            {
                A = a;
                B = b;
                Result = result;
                Operation = operation;
            }
        }

        private List<HistoryEntry> history = new List<HistoryEntry>();

        [MenuItem("Unity Tools/Vector Math")]
        public static void ShowWindow()
        {
            GetWindow<VectorMathWindow>("Vector Math");
        }

        private void OnGUI()
        {
            GUILayout.Label("Vector Math Tool", EditorStyles.boldLabel);

            vectorA = EditorGUILayout.Vector3Field("Vector A", vectorA);
            vectorB = EditorGUILayout.Vector3Field("Vector B", vectorB);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Subtract (A - B)"))
            {
                result = vectorA - vectorB;
                history.Add(new HistoryEntry(vectorA, vectorB, result, "A - B"));
            }

            if (GUILayout.Button("Add (A + B)"))
            {
                result = vectorA + vectorB;
                history.Add(new HistoryEntry(vectorA, vectorB, result, "A + B"));
            }

            if (GUILayout.Button("Invert Inputs (Swap A ↔ B)"))
            {
                (vectorA, vectorB) = (vectorB, vectorA);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Last Result", result.ToString());

            // History section
            GUILayout.Space(10);
            GUILayout.Label("History", EditorStyles.boldLabel);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(220));
            for (int i = 0; i < history.Count; i++)
            {
                var entry = history[i];

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField($"[{i}] {entry.Operation}");
                EditorGUILayout.LabelField($"A: {entry.A}");
                EditorGUILayout.LabelField($"B: {entry.B}");
                EditorGUILayout.LabelField($"Result: {entry.Result}");

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Use Values"))
                {
                    vectorA = entry.A;
                    vectorB = entry.B;
                    result = entry.Result;
                }
                if (GUILayout.Button("Delete"))
                {
                    history.RemoveAt(i);
                    GUIUtility.ExitGUI(); // Prevents layout error after removal
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Clear History"))
            {
                history.Clear();
            }
        }
    }


}