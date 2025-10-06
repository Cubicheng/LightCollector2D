using UnityEngine;

public class TitleFloating : MonoBehaviour {
    public float floatSpeed = 1f;       // 浮动速度
    public float floatAmount = 0.5f;    // 浮动幅度
    public float rotationAmount = 2f;   // 旋转幅度

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float randomOffset; // 用于使每个对象的动画不同

    void Start() {
        startPosition = transform.position;
        startRotation = transform.rotation;
        randomOffset = Random.Range(0f, 100f); // 随机偏移量
    }

    void Update() {
        // 使用Perlin噪声计算浮动值
        float noiseX = Mathf.PerlinNoise(randomOffset, Time.time * floatSpeed);
        float noiseY = Mathf.PerlinNoise(randomOffset + 1f, Time.time * floatSpeed);
        float noiseZ = Mathf.PerlinNoise(randomOffset + 2f, Time.time * floatSpeed);

        // 应用位置浮动
        Vector3 newPosition = startPosition;
        newPosition.x += (noiseX - 0.5f) * floatAmount;
        newPosition.y += (noiseY - 0.5f) * floatAmount;
        newPosition.z += (noiseZ - 0.5f) * floatAmount;
        transform.position = newPosition;

        // 应用轻微旋转
        Quaternion newRotation = startRotation;
        newRotation *= Quaternion.Euler(
            (noiseX - 0.5f) * rotationAmount,
            (noiseY - 0.5f) * rotationAmount,
            (noiseZ - 0.5f) * rotationAmount
        );
        transform.rotation = newRotation;
    }
}