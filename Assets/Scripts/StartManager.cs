using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject myCamera;

    private void Update() {
        if (Input.anyKeyDown) {
            player.SetActive(true);
            myCamera.GetComponent<SmoothCameraFollow>().enabled = true;
            GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
            foreach (GameObject checkPoint in checkPoints) {
                checkPoint.GetComponent<CheckPoint>().RegisterPlayer();
            }
        }
    }
}
