using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Experimental.AI;

namespace UnityTools.EditorUtils
{
    public class RandomRotatorWindow : EditorWindow
    {
        private List<GameObject> objectsToRotate = new List<GameObject>();
        private Vector3 minRotation = Vector3.zero;
        private Vector3 maxRotation = new Vector3(360, 360, 360);

        private bool useDiscreteAngles = false;

        private bool randomizeX = true;
        private bool randomizeY = true;
        private bool randomizeZ = true;

        private readonly float[] discreteAngles = new float[] { 0f, 90f, 180f, 270f };
        private Vector2 scrollPos;
        private bool scrollFoldout;

        [MenuItem("Unity Tools/Random Rotator")]
        public static void ShowWindow()
        {
            GetWindow<RandomRotatorWindow>("Random Rotator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Random Rotation Tool", EditorStyles.boldLabel);
            using (new EditorGUIUtil.BoxScope("Objects"))
            {
                // Objects array with populate button
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Objects to Rotate", GUILayout.Width(120));

                if (GUILayout.Button("Use Selection", GUILayout.Width(100)))
                {
                    objectsToRotate.Clear();
                    foreach (var obj in Selection.gameObjects)
                    {
                        if (!objectsToRotate.Contains(obj))
                            objectsToRotate.Add(obj);
                    }
                }
                EditorGUILayout.EndHorizontal();

                // Show current objects in a scrollable list
                if (objectsToRotate.Count > 0)
                {
                    scrollFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(scrollFoldout, "Objects to Rotate");
                    EditorGUILayout.EndFoldoutHeaderGroup();
                    if (scrollFoldout)
                    {
                        using (var scroll = new EditorGUILayout.ScrollViewScope(scrollPos))
                        {
                            scrollPos = scroll.scrollPosition;

                            for (int i = 0; i < objectsToRotate.Count; i++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                objectsToRotate[i] = (GameObject) EditorGUILayout.ObjectField(objectsToRotate[i], typeof(GameObject), true);
                                if (GUILayout.Button("X", GUILayout.Width(20)))
                                {
                                    objectsToRotate.RemoveAt(i);
                                    i--;
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("No objects added.", MessageType.Info);
                }
            }
            // Axis toggles
            using (new EditorGUIUtil.BoxScope("Axis Options"))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.PrefixLabel("Randomize");
                    GUILayout.Label("X", GUILayout.ExpandWidth(false));
                    randomizeX = EditorGUILayout.Toggle(randomizeX, GUILayout.ExpandWidth(false));
                    GUILayout.Label("Y", GUILayout.ExpandWidth(false));
                    randomizeY = EditorGUILayout.Toggle(randomizeY, GUILayout.ExpandWidth(false));
                    GUILayout.Label("Z", GUILayout.ExpandWidth(false));
                    randomizeZ = EditorGUILayout.Toggle(randomizeZ, GUILayout.ExpandWidth(false));
                }

                // Discrete toggle
                useDiscreteAngles = EditorGUILayout.Toggle("Use 0/90/180/270", useDiscreteAngles);

                if (!useDiscreteAngles)
                {
                    minRotation = EditorGUILayout.Vector3Field("Min Rotation", minRotation);
                    maxRotation = EditorGUILayout.Vector3Field("Max Rotation", maxRotation);
                }
            }
            GUILayout.Space(10);

            if (GUILayout.Button("Randomize Rotations"))
            {
                ApplyRandomRotation();
            }

            if (GUILayout.Button("Clear List"))
            {
                objectsToRotate.Clear();
            }
        }

        private void ApplyRandomRotation()
        {
            foreach (GameObject obj in objectsToRotate)
            {
                if (obj == null) continue;

                Undo.RecordObject(obj.transform, "Random Rotate Objects");

                Vector3 currentRot = obj.transform.eulerAngles;
                Vector3 newRot = currentRot;

                if (randomizeX)
                    newRot.x = useDiscreteAngles
                        ? discreteAngles[Random.Range(0, discreteAngles.Length)]
                        : Random.Range(minRotation.x, maxRotation.x);

                if (randomizeY)
                    newRot.y = useDiscreteAngles
                        ? discreteAngles[Random.Range(0, discreteAngles.Length)]
                        : Random.Range(minRotation.y, maxRotation.y);

                if (randomizeZ)
                    newRot.z = useDiscreteAngles
                        ? discreteAngles[Random.Range(0, discreteAngles.Length)]
                        : Random.Range(minRotation.z, maxRotation.z);

                obj.transform.rotation = Quaternion.Euler(newRot);
            }
        }
    }
}
