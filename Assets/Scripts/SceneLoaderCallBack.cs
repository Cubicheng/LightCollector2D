using UnityEngine;

public class LoaderCallback : MonoBehaviour {

    private bool isFirstUpdated = false;

    void Update() {
        if (isFirstUpdated) {
            return;
        }
        isFirstUpdated = true;
        SceneLoader.LoaderCallback();
    }
}
