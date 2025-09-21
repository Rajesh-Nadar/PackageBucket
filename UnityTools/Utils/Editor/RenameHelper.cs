using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace UnityTools.EditorUtils
{
    public static class RenameHelper
    {
        [MenuItem("Unity Tools/Rename Helper/Remove Snake Case")]
        static void RemoveSnakeCase()
        {
            foreach (var selectedObject in Selection.gameObjects)
            {
                Undo.RecordObject(selectedObject, "Remove Snake Case");
                selectedObject.name = selectedObject.name.Replace("_", " ");
            }
        }

        [MenuItem("Unity Tools/Rename Helper/Capitalize Each Word")]
        static void CapitalizeEachWord()
        {
            foreach (var selectedObject in Selection.gameObjects)
            {
                Undo.RecordObject(selectedObject, "Capitalize Each Word");

                // Get the TextInfo object for the current culture
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                selectedObject.name = textInfo.ToTitleCase(selectedObject.name);
            }
        }

        [MenuItem("Unity Tools/Rename Helper/Remove Duplicate Numbers")]
        static void RemoveDuplicateNumbers()
        {
            foreach (var selectedObject in Selection.gameObjects)
            {
                Undo.RecordObject(selectedObject, "Remove Duplicate Numbers");

                string name = selectedObject.name;

                int startIndex = name.LastIndexOf("(");
                int endIndex = name.LastIndexOf(")");

                if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                {
                    // Remove the (1), (2), etc.
                    string cleaned = name.Substring(0, startIndex).TrimEnd();
                    selectedObject.name = cleaned;
                }
            }
        }
    }
}
