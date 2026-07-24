using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class StartSceneAssigner
{
    private const string PATH = "Assets/Scenes/Main Menu.unity";

    static StartSceneAssigner()
    {
        SceneAsset startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(PATH);

        if (startScene != null) EditorSceneManager.playModeStartScene = startScene;
        else Debug.LogWarning($"Could not find start scene at path: {PATH}");
    }
}