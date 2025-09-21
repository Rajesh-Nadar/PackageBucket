using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityTools.EditorUtils;

namespace UnityTools.Utils
{
    public class ObjectReplacerWindow : EditorWindow
    {
        private GameObject replacementPrefabOrObject;
        private GameObject searchPrefab;

        // Group → Single
        private List<GameObject> groupPrefabs = new List<GameObject>();
        private GameObject singleReplacementPrefab;

        // Single → Group
        [System.Serializable]
        private class ReplacingObject
        {
            public GameObject replacement;
            [Range(0, 1)] public float chance;
        }
        private GameObject singleSearchPrefab;
        private List<ReplacingObject> randomReplacementPrefabs = new List<ReplacingObject>();

        [MenuItem("Unity Tools/Object Replacer")]
        public static void ShowWindow()
        {
            GetWindow<ObjectReplacerWindow>("Object Replacer");
        }

        private void OnGUI()
        {
            using (new EditorGUIUtil.BoxScope("Replace Selected Objects"))
            {
                EditorGUILayout.HelpBox("1. Select object that you want to replace in the hierarchy.\n2. Select the prefab with you will be replacing in the  \"Replacement Object / Prefab\" field.\n3. Press \"Replace Selected\" button to replace.", MessageType.Info);
                replacementPrefabOrObject = (GameObject) EditorGUILayout.ObjectField(
                    "Replacement Object / Prefab",
                    replacementPrefabOrObject,
                    typeof(GameObject),
                    true
                );

                if (GUILayout.Button("Replace Selected"))
                    ReplaceSelected();
            }

            GUILayout.Space(20);
            using (new EditorGUIUtil.BoxScope("Search and Replace by Prefab"))
            {
                searchPrefab = (GameObject) EditorGUILayout.ObjectField(
                    "Search Prefab",
                    searchPrefab,
                    typeof(GameObject),
                    false
                );

                if (GUILayout.Button("Replace All Instances in Scene"))
                    ReplaceAllPrefabInstances();
            }

            GUILayout.Space(20);
            using (new EditorGUIUtil.BoxScope("Group → Single Replacement"))
            {
                int newGroupSize = Mathf.Max(0, EditorGUILayout.IntField("Group Size", groupPrefabs.Count));
                while (newGroupSize > groupPrefabs.Count) groupPrefabs.Add(null);
                while (newGroupSize < groupPrefabs.Count) groupPrefabs.RemoveAt(groupPrefabs.Count - 1);

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    for (int i = 0; i < groupPrefabs.Count; i++)
                    {
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                            groupPrefabs[i] = (GameObject) EditorGUILayout.ObjectField($"Group Prefab {i + 1}", groupPrefabs[i], typeof(GameObject), false);
                        }
                    }
                }

                singleReplacementPrefab = (GameObject) EditorGUILayout.ObjectField("Single Replacement Prefab", singleReplacementPrefab, typeof(GameObject), false);

                if (GUILayout.Button("Replace Group With Single"))
                    ReplaceGroupWithSingle();
            }

            GUILayout.Space(20);
            using (new EditorGUIUtil.BoxScope("Single → Group Replacement"))
            {
                singleSearchPrefab = (GameObject) EditorGUILayout.ObjectField("Single Search Prefab", singleSearchPrefab, typeof(GameObject), false);

                int newRandSize = Mathf.Max(0, EditorGUILayout.IntField("Random Pool Size", randomReplacementPrefabs.Count));
                while (newRandSize > randomReplacementPrefabs.Count) randomReplacementPrefabs.Add(null);
                while (newRandSize < randomReplacementPrefabs.Count) randomReplacementPrefabs.RemoveAt(randomReplacementPrefabs.Count - 1);
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    for (int i = 0; i < randomReplacementPrefabs.Count; i++)
                    {
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                            randomReplacementPrefabs[i].replacement = (GameObject) EditorGUILayout.ObjectField($"Random Prefab {i + 1}", randomReplacementPrefabs[i].replacement, typeof(GameObject), false);
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.PrefixLabel("Weight");
                                randomReplacementPrefabs[i].chance = EditorGUILayout.Slider(randomReplacementPrefabs[i].chance, 0, 1);
                            }
                        }
                    }
                }
                if (GUILayout.Button("Replace Single With Random Group"))
                    ReplaceSingleWithGroup();
            }
        }

        private void ReplaceSelected()
        {
            if (replacementPrefabOrObject == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a replacement object or prefab.", "OK");
                return;
            }

            foreach (GameObject selected in Selection.gameObjects)
            {
                ReplaceOne(selected, replacementPrefabOrObject);
            }
        }

        private void ReplaceAllPrefabInstances()
        {
            if (searchPrefab == null || replacementPrefabOrObject == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign both a Search Prefab and a Replacement Object.", "OK");
                return;
            }

            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            int count = 0;

            foreach (var obj in allObjects)
            {
                if (PrefabUtility.GetCorrespondingObjectFromSource(obj) == searchPrefab)
                {
                    ReplaceOne(obj, replacementPrefabOrObject);
                    count++;
                }
            }

            EditorUtility.DisplayDialog("Done", $"Replaced {count} objects.", "OK");
        }

        private void ReplaceGroupWithSingle()
        {
            if (groupPrefabs.Count == 0 || singleReplacementPrefab == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a group of prefabs and a single replacement.", "OK");
                return;
            }

            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            int count = 0;

            foreach (var obj in allObjects)
            {
                GameObject src = PrefabUtility.GetCorrespondingObjectFromSource(obj);
                if (src != null && groupPrefabs.Contains(src))
                {
                    ReplaceOne(obj, singleReplacementPrefab);
                    count++;
                }
            }

            EditorUtility.DisplayDialog("Done", $"Replaced {count} objects.", "OK");
        }

        private void ReplaceSingleWithGroup()
        {
            if (singleSearchPrefab == null || randomReplacementPrefabs.Count == 0)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a single search prefab and a random group.", "OK");
                return;
            }

            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            int count = 0;

            foreach (var obj in allObjects)
            {
                if (PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj) ==
                    PrefabUtility.GetCorrespondingObjectFromOriginalSource(singleSearchPrefab))
                {
                    GameObject randPrefab = GetRandomWeightedPrefab();
                    if (randPrefab != null)
                    {
                        ReplaceOne(obj, randPrefab);
                        count++;
                    }
                }
            }

            EditorUtility.DisplayDialog("Done", $"Replaced {count} objects. {allObjects.Length}", "OK");
        }

        GameObject GetRandomWeightedPrefab()
        {
            if (randomReplacementPrefabs == null || randomReplacementPrefabs.Count == 0)
                return null;

            float totalWeight = 0f;
            foreach (var wp in randomReplacementPrefabs)
                totalWeight += wp.chance;

            float randomValue = Random.value * totalWeight;

            foreach (var wp in randomReplacementPrefabs)
            {
                if (randomValue < wp.chance)
                    return wp.replacement;
                randomValue -= wp.chance;
            }

            return randomReplacementPrefabs[0].replacement; // fallback
        }

        private void ReplaceOne(GameObject original, GameObject replacement)
        {
            if (original == null || replacement == null) return;

            Undo.RegisterFullObjectHierarchyUndo(original, "Replace Object");

            Transform parent = original.transform.parent;
            GameObject newObj = InstantiateReplacement(replacement);

            if (newObj != null)
            {
                Undo.RegisterCreatedObjectUndo(newObj, "Created Replacement");
                newObj.transform.SetPositionAndRotation(original.transform.position, original.transform.rotation);
                newObj.transform.localScale = original.transform.localScale;
                if (parent != null) newObj.transform.SetParent(parent, true);
                Undo.DestroyObjectImmediate(original);
            }
        }

        private GameObject InstantiateReplacement(GameObject prefabOrObject)
        {
            if (PrefabUtility.IsPartOfPrefabAsset(prefabOrObject))
            {
                return (GameObject) PrefabUtility.InstantiatePrefab(prefabOrObject);
            }
            else
            {
                return Instantiate(prefabOrObject);
            }
        }
    }
}
