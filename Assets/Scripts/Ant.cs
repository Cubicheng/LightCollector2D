using UnityEngine;

public class Ant : MonoBehaviour {
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float patrolRange = 5f;
    [SerializeField] private float startDirection = -1f;
    [SerializeField] private GameObject flipRoot; // 仅用于视觉翻转

    private Vector2 startPosition;
    private float currentDirection;
    private bool isFacingRight;

    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;

    private void Awake() {
        // 记录初始位置（使用父对象的位置）
        startPosition = transform.position;
        currentDirection = Mathf.Sign(startDirection);
        isFacingRight = currentDirection > 0;
    }

    private void Update() {
        PatrolMovement();
        UpdateSpriteDirection();
    }

    private void PatrolMovement() {
        // 计算目标位置（基于初始位置和方向）
        float targetX = startPosition.x + (currentDirection * patrolRange);

        // 移动父对象
        transform.position = Vector2.MoveTowards(
            transform.position,
            new Vector2(targetX, transform.position.y),
            moveSpeed * Time.deltaTime
        );

        // 更精确的边界检测
        float distanceFromStart = transform.position.x - startPosition.x;
        if (Mathf.Abs(distanceFromStart) >= patrolRange) {
            currentDirection *= -1;
            isFacingRight = !isFacingRight;
        }
    }

    private void UpdateSpriteDirection() {
        // 只翻转视觉部分，不影响移动逻辑
        if (flipRoot != null) {
            Vector3 scale = flipRoot.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (isFacingRight ? 1 : -1);
            flipRoot.transform.localScale = scale;
        }
    }

    private void OnDrawGizmosSelected() {
        if (!showGizmos) return;

        Vector2 origin = Application.isPlaying ? startPosition : (Vector2)transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            origin + Vector2.left * patrolRange,
            origin + Vector2.right * patrolRange
        );

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(origin + Vector2.left * patrolRange, 0.2f);
        Gizmos.DrawSphere(origin + Vector2.right * patrolRange, 0.2f);
    }
}