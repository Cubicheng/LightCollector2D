using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader {
    public enum Scene {
        dev0,
        dev1,
        dev2,
        Loading
    };

    private static Scene targetScene;

    public static void GoToNextLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        if (nextSceneIndex >= sceneCount) {
            nextSceneIndex = 0;
        }

        targetScene = (Scene)nextSceneIndex;
        LoadScene(targetScene);
    }

    public static void LoadScene(Scene targetScene) {
        SceneLoader.targetScene = targetScene;
        SceneManager.LoadScene((int)Scene.Loading);
    }

    public static void LoaderCallback() {
        SceneManager.LoadScene((int)targetScene);
    }
}