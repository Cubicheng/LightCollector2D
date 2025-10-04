using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour {
    [SerializeField] private float speed = 500f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravityScale = 3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int maxAirJumps = 1;
    [SerializeField] private float airJumpForce = 20f;
    [SerializeField] private float lightOuterRadiusFadingSpeed = 1.0f;
    [SerializeField] private Animator animator;
    [SerializeField] private float pikaDuration = 0.5f;
    [SerializeField] private GameObject holyLight;
    [SerializeField] private GameObject lightBall;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isGrounded;
    private int airJumpCount;
    private float horizontalValue;
    private float facingValue = 1;

    private bool isWalk = false;
    private bool isJump = false;
    private bool isAirJump = false;
    private bool isPika = false;
    private bool isThrow = false;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    [SerializeField] private Light2D light2d;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.freezeRotation = true;

        coll = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        horizontalValue = Input.GetAxisRaw("Horizontal");
        if (horizontalValue!=0 && horizontalValue != facingValue) {
            facingValue = horizontalValue;
            if (facingValue==1) {
                spriteRenderer.flipX = false;
            } else {
                spriteRenderer.flipX = true;
            }
        }
        if ((Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.Space)) && isGrounded) {
            Jump();
        }else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && CanAirJump()) {
            AirJump();
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            Pika();
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            Throw();
        }
    }

    private void Throw() {
        Instantiate(
            lightBall,
            transform.position,
            Quaternion.identity
        );
    }

    private void Pika() {
        if (isPika || isWalk || isJump|| isAirJump || isThrow) {
            return;
        }
        animator.SetBool(AnimatorParams.IsPika, isPika = true);
        holyLight.SetActive(isPika);
        StartCoroutine(ResetPikaAfterDelay(pikaDuration));
    }

    IEnumerator ResetPikaAfterDelay(float delayTime) {
        yield return new WaitForSeconds(delayTime);
        animator.SetBool(AnimatorParams.IsPika, isPika = false);
        holyLight.SetActive(isPika);
    }

    private void FixedUpdate() {
        bool wasGrounded = isGrounded;
        isGrounded = CheckGrounded();

        if (!wasGrounded && isGrounded) {
            airJumpCount = 0;
            animator.SetBool(AnimatorParams.IsJump, isJump = false);
            animator.SetBool(AnimatorParams.IsAirJump, isAirJump = false);
        }
        Move();
        UpdateLight();
    }

    private void UpdateLight() {
        light2d.pointLightOuterRadius -= lightOuterRadiusFadingSpeed * Time.fixedDeltaTime;
        if (light2d.pointLightOuterRadius <= 0) {
            light2d.pointLightOuterRadius = 0;
        }
    }

    private bool CanAirJump() {
        return airJumpCount < maxAirJumps;
    }

    private void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetBool(AnimatorParams.IsJump,isJump = true);
    }

    private void AirJump() {
        airJumpCount++;
        float preservedXVelocity = rb.velocity.x * 0.8f;
        rb.velocity = new Vector2(preservedXVelocity, airJumpForce);
        animator.SetBool(AnimatorParams.IsAirJump,isAirJump = true);
    }

    private void Move() {
        float xVal = horizontalValue * speed * Time.deltaTime;
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        rb.velocity = targetVelocity;
        animator.SetBool(AnimatorParams.IsWalk, xVal != 0);
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

    public float GetFacingValue() {
        return facingValue;
    }
}