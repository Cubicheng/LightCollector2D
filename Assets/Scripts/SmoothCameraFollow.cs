using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour {
    [Header("��������")]
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 baseOffset = new Vector3(5, 0, -10);

    [Header("��������")]
    public float deadZoneThreshold = 0.5f; // ������ֵ����λ���룩
    public float minDirectionTime = 0.5f;   // ��С���ַ���ʱ��

    private Vector3 currentOffset;
    private float lastDirectionChangeTime;
    private int lastFacingDirection = 1; // 1��ʾ�ң�-1��ʾ��

    private void Start() {
        // ��ʼ��ƫ����Ϊ�Ҳ�
        currentOffset = baseOffset;
        lastDirectionChangeTime = Time.time;
    }

    private void FixedUpdate() {
        if (target == null) return;

        int currentFacing = target.GetComponent<Player>().GetFacingValue() >= 0 ? 1 : -1;

        // ��ⷽ��仯
        if (currentFacing != lastFacingDirection) {
            // ֻ�г�������ʱ�������ı䷽��
            if (Time.time - lastDirectionChangeTime >= deadZoneThreshold) {
                lastFacingDirection = currentFacing;
                lastDirectionChangeTime = Time.time;
            }
        }

        // ����Ŀ��ƫ����
        Vector3 targetOffset = new Vector3(
            baseOffset.x * lastFacingDirection,
            baseOffset.y,
            baseOffset.z
        );

        // ƽ������ƫ����
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, smoothSpeed * Time.fixedDeltaTime * 10);

        // ���㲢Ӧ�������λ��
        Vector3 desiredPosition = target.position + currentOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}