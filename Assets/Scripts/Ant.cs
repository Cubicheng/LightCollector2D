using UnityEngine;

public class Ant : MonoBehaviour {
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float patrolRange = 5f;
    [SerializeField] private float startDirection = -1f;
    [SerializeField] private GameObject flipRoot; // �������Ӿ���ת

    private Vector2 startPosition;
    private float currentDirection;
    private bool isFacingRight;

    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;

    private void Awake() {
        // ��¼��ʼλ�ã�ʹ�ø������λ�ã�
        startPosition = transform.position;
        currentDirection = Mathf.Sign(startDirection);
        isFacingRight = currentDirection > 0;
    }

    private void Update() {
        PatrolMovement();
        UpdateSpriteDirection();
    }

    private void PatrolMovement() {
        // ����Ŀ��λ�ã����ڳ�ʼλ�úͷ���
        float targetX = startPosition.x + (currentDirection * patrolRange);

        // �ƶ�������
        transform.position = Vector2.MoveTowards(
            transform.position,
            new Vector2(targetX, transform.position.y),
            moveSpeed * Time.deltaTime
        );

        // ����ȷ�ı߽���
        float distanceFromStart = transform.position.x - startPosition.x;
        if (Mathf.Abs(distanceFromStart) >= patrolRange) {
            currentDirection *= -1;
            isFacingRight = !isFacingRight;
        }
    }

    private void UpdateSpriteDirection() {
        // ֻ��ת�Ӿ����֣���Ӱ���ƶ��߼�
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