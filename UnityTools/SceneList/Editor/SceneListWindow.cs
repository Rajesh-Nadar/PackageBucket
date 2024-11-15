using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityTools
{
    public class SceneListWindow : EditorWindow
    {
        private List<string> sceneNames = new List<string>();
        private List<string> scenePaths = new List<string>();
        private string searchQuery = "";
        private Vector2 scroll;

        private GUIContent refreshIcon;
        private GUIContent saveIcon;
        private GUIContent clearIcon;
        private GUIContent restoreIcon;
        private Texture2D sceneIcon;
        private GUIContent addIcon;

        [MenuItem("Unity Tools/Scene List")]
        static void Init()
        {
            SceneListWindow window = (SceneListWindow)EditorWindow.GetWindow(typeof(SceneListWindow));
            window.titleContent = new GUIContent("Scene List");
            window.Show();
        }

        private void OnEnable()
        {
            RefreshSceneList();
            RestoreSearch();

            refreshIcon = EditorGUIUtility.IconContent("d_RotateTool");
            saveIcon = EditorGUIUtility.IconContent("SaveActive");
            restoreIcon = EditorGUIUtility.IconContent("d_FolderOpened Icon");
            clearIcon = EditorGUIUtility.IconContent("close");
            addIcon = EditorGUIUtility.IconContent("d_CreateAddNew");
            sceneIcon = EditorGUIUtility.IconContent("SceneAsset Icon").image as Texture2D;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Scene List", EditorStyles.boldLabel);

            if (GUILayout.Button(refreshIcon, EditorStyles.toolbarButton))
            {
                RefreshSceneList();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            searchQuery = EditorGUILayout.TextField("Search", searchQuery);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                if (GUILayout.Button(clearIcon, EditorStyles.toolbarButton, GUILayout.Width(30)))
                {
                    searchQuery = "";
                }
                if (GUILayout.Button(saveIcon, EditorStyles.toolbarButton, GUILayout.Width(30)))
                {
                    SaveSearch();
                }
            }
            else
            {
                if (GUILayout.Button(restoreIcon, EditorStyles.toolbarButton, GUILayout.Width(30)))
                {
                    RestoreSearch();
                }
            }
            EditorGUILayout.EndHorizontal();

            scroll = EditorGUILayout.BeginScrollView(scroll);

            for (int i = 0; i < sceneNames.Count; i++)
            {
                string sceneName = sceneNames[i];

                bool draw = false;

                string[] query = searchQuery.ToLower().Split(",");

                for (int q = 0; q < query.Length; q++)
                {
                    string qr = query[q].Trim().ToLower();

                    if (!string.IsNullOrEmpty(qr))
                    {
                        if (sceneName.ToLower().Contains(qr))
                            draw = true;
                    }
                }

                if (searchQuery.Length == 0)
                    draw = true;

                if (draw)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.button);

                    GUILayout.Label(sceneIcon, GUILayout.Width(18), GUILayout.Height(18));
                    if (GUILayout.Button(sceneName, GUI.skin.label))
                    {
                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        {
                            EditorSceneManager.OpenScene(scenePaths[i]);
                        }
                    }
                    if (GUILayout.Button(addIcon, GUI.skin.label, GUILayout.Width(20)))
                    {
                        EditorSceneManager.SetActiveScene(EditorSceneManager.OpenScene(scenePaths[i], OpenSceneMode.Additive));
                    }
                    if (EditorSceneManager.sceneCount > 1)
                    {
                        Scene scene = EditorSceneManager.GetSceneByPath(scenePaths[i]);
                        Scene activeScene = default;

                        int sceneCount = EditorSceneManager.loadedSceneCount;

                        for (int k = 0; k < sceneCount; k++)
                        {
                            activeScene = EditorSceneManager.GetSceneAt(k);

                            if (scene == activeScene)
                                break;
                        }

                        if (scene == activeScene)
                        {
                            if (GUILayout.Button(clearIcon, GUI.skin.label, GUILayout.Width(20)))
                            {
                                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                                {
                                    EditorSceneManager.CloseScene(scene, true);
                                }
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void SaveSearch()
        {
            PlayerPrefs.SetString("sceneListSearchQuery", searchQuery);
        }
        private void RestoreSearch()
        {
            searchQuery = PlayerPrefs.GetString("sceneListSearchQuery", "");
        }

        private void RefreshSceneList()
        {
            sceneNames.Clear();
            scenePaths.Clear();

            int sceneCount = EditorSceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < sceneCount; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                sceneNames.Add(sceneName);
                scenePaths.Add(scenePath);
            }
        }
    }
}