using UnityEngine;
using UnityEditor;

public class EditModeFunctions : EditorWindow
{
    [MenuItem("Window/Edit Mode Functions")]
    public static void ShowWindow()
    {
        GetWindow<EditModeFunctions>("World Gen Stuff");
    }

    public static GameManager manager;

    private void OnGUI()
    {
        if (GUILayout.Button("Regenerate"))
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            manager?.Regenerate();
        }
        if (GUILayout.Button("Clear"))
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            manager?.generator.Clear();
        }
    }
}