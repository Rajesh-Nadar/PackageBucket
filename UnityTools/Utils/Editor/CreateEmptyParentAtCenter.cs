using UnityEngine;
using UnityEditor;

namespace UnityTools.EditorUtils
{
    public static class CreateEmptyParentAtCenter
    {
        // Add to GameObject menu, priority 0 ensures top of the list
        [MenuItem("GameObject/Create Empty Parent at Center %g", false, 0)]
        private static void CreateCenterGameObject()
        {
            Transform[] selectedTransforms = Selection.transforms;

            if (selectedTransforms.Length == 0)
            {
                EditorUtility.DisplayDialog("No Selection", "Please select at least one GameObject.", "OK");
                return;
            }

            // Calculate center position
            Vector3 center = Vector3.zero;
            foreach (Transform t in selectedTransforms)
                center += t.position;
            center /= selectedTransforms.Length;

            // Create the center object with Undo support
            GameObject centerObject = new GameObject("Gameobject");
            Undo.RegisterCreatedObjectUndo(centerObject, "Create Center Object");
            centerObject.transform.position = center;

            // Parent all selected objects under the center object (only once)
            foreach (Transform t in selectedTransforms)
            {
                Undo.SetTransformParent(t, centerObject.transform, "Parent to Center Object");
            }

            // Select the new center object
            Selection.activeGameObject = centerObject;
        }

        // Enable menu only if objects are selected
        [MenuItem("GameObject/Create Empty Parent at Center %g", true)]
        private static bool ValidateCreateCenterGameObject()
        {
            return Selection.transforms.Length > 0;
        }

        [MenuItem("GameObject/Break Parent", false, 0)]
        private static void BreakParent()
        {
            if (Selection.activeObject == null) return;
            var selectedObject = (GameObject) Selection.activeObject;
            if (selectedObject == null) return;

            var parent = selectedObject.transform.parent;
            selectedObject.transform.parent = parent.parent;
        }
    }
}