using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class Beetle : MonoBehaviour {
    [Header("���ռ������")]
    public float lightIntensityThreshold = 0.5f; // ��������Ĺ���ǿ����ֵ
    public float detectionRadius = 2f; // ������Դ�ļ��뾶
    public float lightCheckInterval = 0.1f; // ���ռ�������룩

    [Header("��������")]
    public float fallSpeed = 3f; // �����ٶ�
    public bool isFalling = false; // �Ƿ����ڵ���

    private Rigidbody2D rb;
    private Collider2D beetleCollider;
    private List<Light2D> trackedLights = new List<Light2D>(); // ֻ�����ֶ���ӵĹ�Դ
    private float nextLightCheckTime;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        beetleCollider = GetComponent<Collider2D>();
        rb.isKinematic = true; // ��ʼʱ��������

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        AddLightToDetection(player.GetComponent<Player>().GetLight2D());
    }

    void Update() {
        if (!isFalling && Time.time >= nextLightCheckTime) {
            nextLightCheckTime = Time.time + lightCheckInterval;
            if (CheckTrackedLights()) {
                StartFalling();
            }
        }
    }

    // ���Ҫ���Ĺ�Դ
    public void AddLightToDetection(Light2D light) {
        if (light != null && !trackedLights.Contains(light)) {
            trackedLights.Add(light);
        }
    }

    // �Ƴ����ټ��Ĺ�Դ
    public void RemoveLightFromDetection(Light2D light) {
        if (trackedLights.Contains(light)) {
            trackedLights.Remove(light);
        }
    }

    // ������б����ٵĹ�Դ
    bool CheckTrackedLights() {
        foreach (var light in trackedLights) {
            if (light != null && CheckSingleLight(light)) {
                return true;
            }
        }
        return false;
    }

    // ��鵥����Դ
    bool CheckSingleLight(Light2D light) {
        float distance = Vector2.Distance(transform.position, light.transform.position);
        if (distance > detectionRadius) return false;

        Vector2 directionToLight = (light.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToLight, distance);

        if (hit.collider != null && hit.collider != beetleCollider) {
            return false; // ���ϰ����赲
        }

        float intensity = light.intensity * (1 - distance / detectionRadius);
        return intensity >= lightIntensityThreshold;
    }

    void StartFalling() {
        isFalling = true;
        rb.isKinematic = false;
        rb.gravityScale = fallSpeed;
    }

    public void ResetBeetle() {
        isFalling = false;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }
}