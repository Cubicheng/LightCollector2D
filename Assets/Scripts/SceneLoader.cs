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
    }

    public static void LoadScene(Scene targetScene) {
        SceneLoader.targetScene = targetScene;
        SceneManager.LoadScene((int)Scene.Loading);
    }

    public static void LoaderCallback() {
        SceneManager.LoadScene((int)targetScene);
    }
}
