using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class FavoritesWindow : EditorWindow
{
    [System.Serializable]
    public class FavoritesData
    {
        public List<string> favPaths;
    }

    public string PrefPath
    {
        get
        {
            return Application.productName + "_favData";
        }
    }
    public string Name => "Favorites";
    public List<Object> favorites;
    private bool inited;
    private List<string> favPaths;

    [MenuItem("Tools/Favorites")]
    static void Menu()
    {
        EditorWindow.GetWindow(typeof(FavoritesWindow)).Show();
    }
    private int _page = 0;
    private string[] _pages;
    private int _defaultPage=0;

    public void OnGUI()
    {
        if (_pages == null)
        {
            List<string> pages = new List<string>();
            for (int i = 0; i < 9; i++)
            {
                pages.Add($"Page {i}");
            }

            _pages = pages.ToArray();
        }

        GUI.changed = false;
        GUILayout.BeginHorizontal();
        _page = GUILayout.Toolbar(_page, _pages);
        GUILayout.EndHorizontal();
        GUI.changed = true;
        if (GUI.changed)
        {
            Init(_page);
        }


        if (!inited)
        {
            inited = true;
            Init(_page);
        }
        GUILayout.BeginHorizontal();
        if (Selection.activeObject != null && GUILayout.Button("Add"))
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            favorites.Add(Selection.activeObject);
            favPaths.Add(path);
            Save();
        }

        if (GUILayout.Button("Refresh"))
        {
            Init(_page);
        }
        if (GUILayout.Button("Clear"))
        {
            favorites.Clear();
            favPaths.Clear();

        }
        GUILayout.EndHorizontal();

        //foreach (var o in favorites)
        for (int i = 0; i < favorites.Count; i++)
        {
            var o = favorites[i];
            GUILayout.BeginHorizontal();
            var on = EditorGUILayout.ObjectField(o, typeof(UnityEngine.Object), false, GUILayout.Width(300));
            if (on != o)
            {
                string path = AssetDatabase.GetAssetPath(on);
                favPaths[i] = path;
                favorites[i] = on;
                Save();
            }

            if (o == null)
            {

            }
            if (o is SceneAsset)
            {
                if (GUILayout.Button("Open"))
                {
                    var newPath = AssetDatabase.GetAssetPath(o);
                    EditorSceneManager.OpenScene(newPath);
                }
            }
            else if (o is GameObject && PrefabUtility.GetPrefabAssetType(o as GameObject) == PrefabAssetType.Regular)
            {
                if (GUILayout.Button("Open"))
                {
                    var prefabPath = AssetDatabase.GetAssetPath(o);
                    var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                    AssetDatabase.OpenAsset(prefabAsset);
                }
            }
            else if (GUILayout.Button("Select"))
            {
                Selection.activeObject = o;
            }

            if (GUILayout.Button("Select", GUILayout.Width(60)))
            {
                Selection.activeObject = o;
            }
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                favorites.RemoveAt(i);
                favPaths.RemoveAt(i);
                Save();
                i--;
            }
            GUILayout.EndHorizontal();
        }
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(new FavoritesData() { favPaths = favPaths });
        string ppath = PrefPath;
        if (_page != 0)
        {
            ppath = ppath + "_" + _page;
        }

        EditorPrefs.SetString(ppath, json);
    }

    private void Init(int page)
    {
        favorites = new List<Object>();
        favPaths = new List<string>();
        string ppath = PrefPath;
        if (page != 0)
        {
            ppath = ppath + "_" + page;
        }

        string json = EditorPrefs.GetString(ppath);

        if (!string.IsNullOrEmpty(json))
        {
            FavoritesData data = JsonUtility.FromJson<FavoritesData>(json);
            favPaths = data.favPaths;
            foreach (var path in favPaths)
            {
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                favorites.Add(asset);
            }
        }
    }
}


