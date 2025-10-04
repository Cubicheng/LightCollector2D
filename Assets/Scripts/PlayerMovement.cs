using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float speed = 500f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravityScale = 3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int maxAirJumps = 1;
    [SerializeField] private float airJumpForce = 20f;

    private bool isGrounded;
    private int airJumpCount;
    private float horizontalValue;

    private Rigidbody2D rb;
    private BoxCollider2D coll;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.freezeRotation = true;

        coll = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        horizontalValue = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.W) && isGrounded) {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.W) && CanAirJump()) {
            AirJump();
        }
    }

    private void FixedUpdate() {
        bool wasGrounded = isGrounded;
        isGrounded = CheckGrounded();

        if (!wasGrounded && isGrounded) {
            airJumpCount = 0;
        }

        Move();
    }

    private bool CanAirJump() {
        return airJumpCount < maxAirJumps;
    }

    private void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void AirJump() {
        airJumpCount++;
        float preservedXVelocity = rb.velocity.x * 0.8f;
        rb.velocity = new Vector2(preservedXVelocity, airJumpForce);
    }

    private void Move() {
        float xVal = horizontalValue * speed * Time.deltaTime;
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        rb.velocity = targetVelocity;
    }
    private bool CheckGrounded() {
        float extraHeight = 0.1f;
        RaycastHit2D hit = Physics2D.BoxCast(
            coll.bounds.center,
            coll.bounds.size,
            0f,
            Vector2.down,
            extraHeight,
            groundLayer);

        // 可视化调试
        Color rayColor = hit.collider != null ? Color.green : Color.red;
        Debug.DrawRay(coll.bounds.center + new Vector3(coll.bounds.extents.x, 0), Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(coll.bounds.center - new Vector3(coll.bounds.extents.x, 0), Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(coll.bounds.center - new Vector3(coll.bounds.extents.x, coll.bounds.extents.y + extraHeight), Vector2.right * (coll.bounds.extents.x * 2), rayColor);

        return hit.collider != null;
    }
}