using UnityEditor;
using UnityEngine;

namespace UnityTools.EditorUtils
{
    public static class EditorGUIUtil
    {
        public class BoxScope : System.IDisposable
        {
            private readonly EditorGUILayout.VerticalScope outer;
            private readonly EditorGUILayout.VerticalScope inner;

            public BoxScope(string title)
            {
                // Outer container
                outer = new EditorGUILayout.VerticalScope(EditorStyles.helpBox);

                // Title
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);

                // Inner container
                inner = new EditorGUILayout.VerticalScope(GUI.skin.box);
            }

            public void Dispose()
            {
                inner.Dispose();  // closes GUI.skin.box
                outer.Dispose();  // closes helpbox
            }
        }

        public static BoxScope BeginBox(string title)
        {
            return new BoxScope(title);
        }
    }
}