using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class Beetle : MonoBehaviour {
    [Header("光照检测设置")]
    public float lightIntensityThreshold = 0.5f; // 触发掉落的光照强度阈值
    public float detectionRadius = 2f; // 单个光源的检测半径
    public float lightCheckInterval = 0.1f; // 光照检测间隔（秒）

    [Header("掉落设置")]
    public float fallSpeed = 3f; // 掉落速度
    public bool isFalling = false; // 是否正在掉落

    private Rigidbody2D rb;
    private Collider2D beetleCollider;
    private List<Light2D> trackedLights = new List<Light2D>(); // 只跟踪手动添加的光源
    private float nextLightCheckTime;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        beetleCollider = GetComponent<Collider2D>();
        rb.isKinematic = true; // 初始时禁用物理

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

    // 添加要检测的光源
    public void AddLightToDetection(Light2D light) {
        if (light != null && !trackedLights.Contains(light)) {
            trackedLights.Add(light);
        }
    }

    // 移除不再检测的光源
    public void RemoveLightFromDetection(Light2D light) {
        if (trackedLights.Contains(light)) {
            trackedLights.Remove(light);
        }
    }

    // 检查所有被跟踪的光源
    bool CheckTrackedLights() {
        foreach (var light in trackedLights) {
            if (light != null && CheckSingleLight(light)) {
                return true;
            }
        }
        return false;
    }

    // 检查单个光源
    bool CheckSingleLight(Light2D light) {
        float distance = Vector2.Distance(transform.position, light.transform.position);
        if (distance > detectionRadius) return false;

        Vector2 directionToLight = (light.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToLight, distance);

        if (hit.collider != null && hit.collider != beetleCollider) {
            return false; // 有障碍物阻挡
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