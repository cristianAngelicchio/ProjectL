using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData))]
public class LevelDataInspector : Editor
{
    public override void OnInspectorGUI()
    {
        LevelData levelData = (LevelData)target;

        if (GUILayout.Button("Abrir en editor"))
        {
            var window = LevelEditorWindow.ShowWindow();
            window.LoadLevelData(levelData);
        }
    }
}
