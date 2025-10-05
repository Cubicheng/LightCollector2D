using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    private Vector3 currentRespawnPoint;
    private GameObject player;
    private List<GameObject> collectableBallList = new List<GameObject>();
    private List<Beetle> beetleList = new List<Beetle>(); // 改为存储Beetle组件而不是GameObject
    private Dictionary<GameObject, Vector3> beetleInitialPositions = new Dictionary<GameObject, Vector3>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 如果需要跨场景保持
        } else {
            Destroy(gameObject);
        }
    }

    public void RegisterBall(GameObject ball) {
        if (!collectableBallList.Contains(ball)) {
            collectableBallList.Add(ball);
        }
    }

    public void RegisterBeetle(GameObject beetle) {
        Beetle beetleComponent = beetle.GetComponent<Beetle>();
        if (beetleComponent != null && !beetleList.Contains(beetleComponent)) {
            beetleList.Add(beetleComponent);
            // 记录初始位置
            beetleInitialPositions.Add(beetle, beetle.transform.position);
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
        ResetAllBeetles();
    }

    private void ResetCollectableBall() {
        foreach (GameObject obj in collectableBallList) {
            if (obj != null) {
                obj.gameObject.SetActive(true);
            }
        }
    }

    public void ResetAllBeetles() {
        foreach (Beetle beetle in beetleList) {
            if (beetle != null) {
                beetle.ResetBeetle();
                if (beetleInitialPositions.TryGetValue(beetle.gameObject, out Vector3 initialPos)) {
                    beetle.transform.position = initialPos;
                }
            }
        }
    }

    public Vector3 GetCurrentRespawnPoint() {
        return currentRespawnPoint;
    }
}