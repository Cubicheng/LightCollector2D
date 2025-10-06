using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class LightBall : MonoBehaviour {
    [SerializeField] private Light2D light2d;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject player;
    [SerializeField] private float lifeDuration = 5.0f;
    [SerializeField] private float force = 15.0f;

    private List<Beetle> allBeetles = new List<Beetle>();

    private float duration = 0.0f;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();

        // ��ȡ���������м׳�
        Beetle[] beetles = FindObjectsOfType<Beetle>();
        allBeetles = new List<Beetle>(beetles);
    }

    void Start() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        float facingValue = player.GetComponent<Player>().GetFacingValue();
        float playerXSpeed = player.GetComponent<Player>().GetHorizontalSpeed();
        Debug.Log("playerXSpeed" + playerXSpeed);
        rb.velocity = new Vector2(facingValue * force / 1.7f + playerXSpeed / 50.0f, force / 1.7f);

        // �������Դע�ᵽ���м׳�
        RegisterToAllBeetles();
    }

    private void RegisterToAllBeetles() {
        foreach (Beetle beetle in allBeetles) {
            if (beetle != null) {
                beetle.AddLightToDetection(light2d);
                Debug.Log($"Registered light to beetle: {beetle.name}");
            }
        }
    }

    private void FixedUpdate() {
        UpdateLight();
    }

    private void UpdateLight() {
        duration += Time.fixedDeltaTime;
        if (duration>=lifeDuration) {
            // ����ǰ�����м׳����Ƴ���Դ
            UnregisterFromAllBeetles();
            Destroy(gameObject);
        }
    }

    private void UnregisterFromAllBeetles() {
        foreach (Beetle beetle in allBeetles) {
            if (beetle != null) {
                beetle.RemoveLightFromDetection(light2d);
                Debug.Log($"Unregistered light from beetle: {beetle.name}");
            }
        }
    }

    private void OnDestroy() {
        // ȷ����������ʱ�Ƴ���Դ����
        UnregisterFromAllBeetles();
    }
}