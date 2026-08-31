using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelConstructionHelper : EditorWindow
{
    private int _level = 1;
    private string _folderPath = "Assets";

    private readonly List<string> _tags = new()
    {
        "Enemy",
        "SpawnPoint"
    };

    private Vector2 _scrollPosition;

    [MenuItem("CustomWindows/LevelConstructionHelper")]
    public static void ShowWindow()
    {
        GetWindow<LevelConstructionHelper>(false, "Level Construction Helper", true);
    }

    public void OnGUI()
    {
        EditorGUILayout.Space();

        _level = EditorGUILayout.IntField("Level", _level);

        EditorGUILayout.Space();

        DrawFolderSelector();

        EditorGUILayout.Space();

        DrawTags();

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField($"Scene: {SceneManager.GetActiveScene().name}");

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Create Level Data", GUILayout.Height(35)))
        {
            CreateLevelData();
        }
    }

    private void DrawFolderSelector()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Save Folder", GUILayout.Width(80));

        EditorGUILayout.SelectableLabel(_folderPath, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));

        if (GUILayout.Button("Select", GUILayout.Width(60)))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("Select Level Data Folder",Application.dataPath,"");

            if (!string.IsNullOrEmpty(selectedPath))
            {
                if (selectedPath.StartsWith(Application.dataPath))
                {
                    _folderPath ="Assets" + selectedPath.Substring(Application.dataPath.Length);
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Folder", "The folder must be inside Unity Assets folder","OK");
                }
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawTags()
    {
        EditorGUILayout.LabelField("Tags", EditorStyles.boldLabel);

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(120));

        for (int i = 0; i < _tags.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            _tags[i] = EditorGUILayout.TagField(_tags[i]);

            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                _tags.RemoveAt(i);
                i--;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Add Tag"))
        {
            _tags.Add("Untagged");
        }
    }

    private void CreateLevelData()
    {
        if (!AssetDatabase.IsValidFolder(_folderPath))
        {
            EditorUtility.DisplayDialog("Invalid Folder", $"The folder '{_folderPath}' does not exist", "OK");
            return;
        }

        Scene activeScene = SceneManager.GetActiveScene();

        if (!activeScene.isLoaded)
        {
            EditorUtility.DisplayDialog("No Scene", "There is no loaded scene", "OK");
            return;
        }

        List<GameObject> objects = new();

        foreach (string tag in _tags.Distinct())
        {
            if (string.IsNullOrEmpty(tag) || tag == "Untagged") continue;

            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag); ;
            if (taggedObjects.Length == 0) continue;

            objects.AddRange(taggedObjects);
        }

        objects = objects.ToList();

        string assetPath = $"{_folderPath}/Level_{_level}.asset";

        // Check if the asset already exists
        LevelData existingData = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);

        if (existingData != null)
        {
            bool overrideExisting = EditorUtility.DisplayDialog("Level already exists", $"Level {_level} already exists" + $", do you want to override it?", "Override", "Cancel");

            if (!overrideExisting) return;

            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.Refresh();
        }

        // Create level data
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

        levelData.level = _level;
        levelData.objects = objects;

        AssetDatabase.CreateAsset(levelData, assetPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = levelData;

        Debug.Log($"Created Level {_level} with {objects.Count} objects: " + assetPath);
    }
}
