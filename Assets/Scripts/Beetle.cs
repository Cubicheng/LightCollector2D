using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class Beetle : MonoBehaviour {
    [Header("光照检测设置")]
    public float lightCheckInterval = 0.1f; // 光照检测间隔（秒）
    public float radiusMultiplier = 1.0f; // 半径乘数（可调整检测范围大小）

    [Header("掉落设置")]
    public float fallSpeed = 3f; // 掉落速度（单位/秒）
    public bool isFalling = false; // 是否正在掉落

    private Rigidbody2D rb;
    private List<Light2D> trackedLights = new List<Light2D>();
    private float nextLightCheckTime;
    private Vector2 fallDirection = Vector2.down; // 掉落方向（向下）

    private Vector3 initialPosition;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // 初始时禁用物理

        initialPosition = transform.position;

        if (GameManager.Instance != null) {
            GameManager.Instance.RegisterBeetle(this.gameObject);
        }

        // 自动获取玩家光源
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            Player playerComponent = player.GetComponent<Player>();
            if (playerComponent != null) {
                Light2D playerLight = playerComponent.GetLight2D();
                if (playerLight != null) {
                    AddLightToDetection(playerLight);
                }
            }
        }
    }
    public void ResetBeetle() {
        isFalling = false;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        transform.position = initialPosition;
    }

    void Update() {
        if (!isFalling && Time.time >= nextLightCheckTime) {
            nextLightCheckTime = Time.time + lightCheckInterval;

            if (CheckTrackedLights()) {
                StartFalling();
            }
        }

        // 匀速掉落逻辑
        if (isFalling) {
            transform.Translate(fallDirection * fallSpeed * Time.deltaTime);
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
            if (light != null && light.isActiveAndEnabled && CheckSingleLight(light)) {
                return true;
            }
        }
        return false;
    }

    // 检查单个光源（使用光源自身的outerRadius）
    bool CheckSingleLight(Light2D light) {
        float distance = Vector2.Distance(transform.position, light.transform.position);
        float lightRadius = GetLightRadius(light);
        bool isInRange = distance <= lightRadius;

        return isInRange;
    }

    // 获取光源的有效半径（考虑不同类型的Light2D）
    float GetLightRadius(Light2D light) {
        float radius = 0f;

        // 根据光源类型获取对应的半径
        switch (light.lightType) {
            case Light2D.LightType.Point:
                // Point Light使用outerRadius
                radius = light.pointLightOuterRadius;
                break;

            case Light2D.LightType.Freeform:
            case Light2D.LightType.Global:
                // 对于其他类型的光源，使用一个默认值或自定义逻辑
                // 这里使用一个保守的估计值，您可以根据需要调整
                radius = 5f; // 默认半径
                break;

            case Light2D.LightType.Sprite:
                // Sprite Light可能没有outerRadius，使用自定义逻辑
                radius = light.shapeLightParametricRadius;
                break;
        }

        return radius * radiusMultiplier;
    }

    void StartFalling() {
        isFalling = true;
        // 禁用物理组件，因为我们使用Transform直接移动
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }

    // 可视化检测范围（显示每个光源的检测范围）
    void OnDrawGizmosSelected() {
        foreach (var light in trackedLights) {
            if (light != null && light.isActiveAndEnabled) {
                Gizmos.color = Color.yellow;
                float radius = GetLightRadius(light);
                Gizmos.DrawWireSphere(light.transform.position, radius);

                // 绘制从光源到甲虫的连线
                Gizmos.color = Color.red;
                Gizmos.DrawLine(light.transform.position, transform.position);
            }
        }
    }
}