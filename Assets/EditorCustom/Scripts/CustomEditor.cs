using BehaviouralTree;
using UnityEditor;
using UnityEngine;

public class CustomEditor : EditorWindow
{
    [MenuItem("CustomEditor/Custom")]   
    static void Init()
    {
        CustomEditor customEditor =
            (CustomEditor)GetWindow(typeof(CustomEditor), false, "Custom");
    }

    private string botsFolderPath = "Assets/1_MyAssets/Models/Bots";
    int selected = 0;
    void OnGUI()
    {
        selected = GUILayout.SelectionGrid(
            selected,
            new string[] { "A", "B", "C", "D" },
            2
        );
        GUILayout.BeginHorizontal();
        GUILayout.Label("Bots Folder Path", GUILayout.Width(120));
        botsFolderPath = GUILayout.TextField(botsFolderPath);
        if (GUILayout.Button("Select", GUILayout.Width(60)))
        {
            string selected = EditorUtility.OpenFolderPanel("Select Bots Folder", "Assets", "");
            if (!string.IsNullOrEmpty(selected) && selected.StartsWith(Application.dataPath))
            {
                botsFolderPath = "Assets" + selected.Substring(Application.dataPath.Length);
            }
        }
        GUILayout.EndHorizontal();
    }
}
