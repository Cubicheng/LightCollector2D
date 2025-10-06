using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class Beetle : MonoBehaviour {
    [Header("���ռ������")]
    public float lightCheckInterval = 0.1f; // ���ռ�������룩
    public float radiusMultiplier = 1.0f; // �뾶�������ɵ�����ⷶΧ��С��

    [Header("��������")]
    public float fallSpeed = 3f; // �����ٶȣ���λ/�룩
    public bool isFalling = false; // �Ƿ����ڵ���

    private Rigidbody2D rb;
    private List<Light2D> trackedLights = new List<Light2D>();
    private float nextLightCheckTime;
    private Vector2 fallDirection = Vector2.down; // ���䷽�����£�

    private Vector3 initialPosition;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // ��ʼʱ��������

        initialPosition = transform.position;

        if (GameManager.Instance != null) {
            GameManager.Instance.RegisterBeetle(this.gameObject);
        }

        // �Զ���ȡ��ҹ�Դ
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

        // ���ٵ����߼�
        if (isFalling) {
            transform.Translate(fallDirection * fallSpeed * Time.deltaTime);
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
            if (light != null && light.isActiveAndEnabled && CheckSingleLight(light)) {
                return true;
            }
        }
        return false;
    }

    // ��鵥����Դ��ʹ�ù�Դ�����outerRadius��
    bool CheckSingleLight(Light2D light) {
        float distance = Vector2.Distance(transform.position, light.transform.position);
        float lightRadius = GetLightRadius(light);
        bool isInRange = distance <= lightRadius;

        return isInRange;
    }

    // ��ȡ��Դ����Ч�뾶�����ǲ�ͬ���͵�Light2D��
    float GetLightRadius(Light2D light) {
        float radius = 0f;

        // ���ݹ�Դ���ͻ�ȡ��Ӧ�İ뾶
        switch (light.lightType) {
            case Light2D.LightType.Point:
                // Point Lightʹ��outerRadius
                radius = light.pointLightOuterRadius;
                break;

            case Light2D.LightType.Freeform:
            case Light2D.LightType.Global:
                // �����������͵Ĺ�Դ��ʹ��һ��Ĭ��ֵ���Զ����߼�
                // ����ʹ��һ�����صĹ���ֵ�������Ը�����Ҫ����
                radius = 5f; // Ĭ�ϰ뾶
                break;

            case Light2D.LightType.Sprite:
                // Sprite Light����û��outerRadius��ʹ���Զ����߼�
                radius = light.shapeLightParametricRadius;
                break;
        }

        return radius * radiusMultiplier;
    }

    void StartFalling() {
        isFalling = true;
        // ���������������Ϊ����ʹ��Transformֱ���ƶ�
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }

    // ���ӻ���ⷶΧ����ʾÿ����Դ�ļ�ⷶΧ��
    void OnDrawGizmosSelected() {
        foreach (var light in trackedLights) {
            if (light != null && light.isActiveAndEnabled) {
                Gizmos.color = Color.yellow;
                float radius = GetLightRadius(light);
                Gizmos.DrawWireSphere(light.transform.position, radius);

                // ���ƴӹ�Դ���׳������
                Gizmos.color = Color.red;
                Gizmos.DrawLine(light.transform.position, transform.position);
            }
        }
    }
}