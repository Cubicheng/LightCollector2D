using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCG : MonoBehaviour
{
    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            SceneLoader.GoToNextLevel();
        }
    }
}
