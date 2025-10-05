using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector3 currentRespawnPoint;

    private GameObject player;

    private List<GameObject> collectableBallList = new List<GameObject>();
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void RegisterBall(GameObject ball) {
        if (!collectableBallList.Contains(ball)) {
            collectableBallList.Add(ball);
        }
    }

    private void Start() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    public void SetRespawnPoint(Vector3 point) {
        currentRespawnPoint = point;
    }

    public void OnPlayerDead() {
        ResetCollectableBall();
    }

    private void ResetCollectableBall() {
        foreach (GameObject obj in collectableBallList) {
            obj.gameObject.SetActive(true);
        }
    }

    public Vector3 GetCurrentRespawnPoint() {
        return currentRespawnPoint;
    }
}
