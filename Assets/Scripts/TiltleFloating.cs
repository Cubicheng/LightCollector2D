using UnityEngine;

public class TitleFloating : MonoBehaviour {
    public float floatSpeed = 1f;       // �����ٶ�
    public float floatAmount = 0.5f;    // ��������
    public float rotationAmount = 2f;   // ��ת����

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float randomOffset; // ����ʹÿ������Ķ�����ͬ

    void Start() {
        startPosition = transform.position;
        startRotation = transform.rotation;
        randomOffset = Random.Range(0f, 100f); // ���ƫ����
    }

    void Update() {
        // ʹ��Perlin�������㸡��ֵ
        float noiseX = Mathf.PerlinNoise(randomOffset, Time.time * floatSpeed);
        float noiseY = Mathf.PerlinNoise(randomOffset + 1f, Time.time * floatSpeed);
        float noiseZ = Mathf.PerlinNoise(randomOffset + 2f, Time.time * floatSpeed);

        // Ӧ��λ�ø���
        Vector3 newPosition = startPosition;
        newPosition.x += (noiseX - 0.5f) * floatAmount;
        newPosition.y += (noiseY - 0.5f) * floatAmount;
        newPosition.z += (noiseZ - 0.5f) * floatAmount;
        transform.position = newPosition;

        // Ӧ����΢��ת
        Quaternion newRotation = startRotation;
        newRotation *= Quaternion.Euler(
            (noiseX - 0.5f) * rotationAmount,
            (noiseY - 0.5f) * rotationAmount,
            (noiseZ - 0.5f) * rotationAmount
        );
        transform.rotation = newRotation;
    }
}