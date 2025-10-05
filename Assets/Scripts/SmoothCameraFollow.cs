using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour {
    [Header("跟随设置")]
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 baseOffset = new Vector3(5, 0, -10);

    [Header("死区设置")]
    public float deadZoneThreshold = 0.5f; // 死区阈值（单位：秒）
    public float minDirectionTime = 0.5f;   // 最小保持方向时间

    private Vector3 currentOffset;
    private float lastDirectionChangeTime;
    private int lastFacingDirection = 1; // 1表示右，-1表示左

    private void Start() {
        // 初始化偏移量为右侧
        currentOffset = baseOffset;
        lastDirectionChangeTime = Time.time;
    }

    private void FixedUpdate() {
        if (target == null) return;

        int currentFacing = target.GetComponent<Player>().GetFacingValue() >= 0 ? 1 : -1;

        // 检测方向变化
        if (currentFacing != lastFacingDirection) {
            // 只有超过死区时间才允许改变方向
            if (Time.time - lastDirectionChangeTime >= deadZoneThreshold) {
                lastFacingDirection = currentFacing;
                lastDirectionChangeTime = Time.time;
            }
        }

        // 计算目标偏移量
        Vector3 targetOffset = new Vector3(
            baseOffset.x * lastFacingDirection,
            baseOffset.y,
            baseOffset.z
        );

        // 平滑过渡偏移量
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, smoothSpeed * Time.fixedDeltaTime * 10);

        // 计算并应用摄像机位置
        Vector3 desiredPosition = target.position + currentOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}