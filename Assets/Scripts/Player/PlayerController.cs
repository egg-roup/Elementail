using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private float baseMoveSpeed;
    private float baseJumpForce;
 
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
    public float coyoteTime = 0.1f; // Time player can jump after leaving ground
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;
    private bool hasDoubleJumped = false;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.2f;

    private enum DashState { Ready, Dashing, Cooldown }
    private DashState dashState = DashState.Ready;
    private float dashTimer = 0f;
    private bool isDashing = false;
    private bool isInvincible = false;
    private float originalGravity;

    
    // Private variables
    public Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private float coyoteTimeCounter;
    private bool wasGrounded;
    public bool facingRight = true;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        anim = GetComponent<Animator>();
        baseMoveSpeed = moveSpeed;
        baseJumpForce = jumpForce;
        originalGravity = rb.gravityScale;


        // This helps prevent the player from getting stuck on edges
        if (GetComponent<CapsuleCollider2D>() != null)
        {
            Physics2D.queriesHitTriggers = false;
        }
    }

    void Update()
    {
        // Get player horizontal input - use GetAxis for smoother movement
        moveInput = Input.GetAxisRaw("Horizontal");

        // Check for ground
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle coyote time
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Flip player sprite and update facing
        if (moveInput > 0.1f)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            facingRight = true;
        }
        else if (moveInput < -0.1f)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
            facingRight = false;
        }

        // Handle jump input with coyote time
        if (Input.GetButtonDown("Jump"))
        {
            if (coyoteTimeCounter > 0)
            {
                // Regular jump
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                coyoteTimeCounter = 0;
                hasDoubleJumped = false;
            }
            else if (!isGrounded && !hasDoubleJumped)
            {
                // Double jump
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                hasDoubleJumped = true;
            }
        }

        
        // Variable jump height - if player releases jump button, reduce upward velocity
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // Update Animator Parameters
        anim.SetFloat("Speed", Mathf.Abs(moveInput));
        anim.SetBool("IsGrounded", isGrounded);

        if (Input.GetButtonDown("Dash") && dashState == DashState.Ready)
        {
            dashState = DashState.Dashing;
            dashTimer = dashDuration;
            rb.gravityScale = 0;
            isDashing = true;
            isInvincible = false;
        }

    }

    void FixedUpdate()
    {
        // Apply horizontal movement every physics frame
        // Using velocity instead of linearVelocity (which is deprecated)
        float targetVelocityX = moveInput * moveSpeed;
        
        // Apply a small amount of smoothing to avoid abrupt changes
        float currentVelocityX = rb.linearVelocity.x;
        float newVelocityX = Mathf.Lerp(currentVelocityX, targetVelocityX, 0.5f);
        
        rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
        
        // Debug log if we detect potential stuck conditions
        if (Mathf.Abs(moveInput) > 0.5f && Mathf.Abs(rb.linearVelocity.x) < 0.1f && isGrounded)
        {
            Debug.LogWarning("Player might be stuck! Input: " + moveInput + ", Velocity: " + rb.linearVelocity);
        }

        if (dashState == DashState.Dashing)
        {
            // Lock movement and apply dash force
            float dashDir = facingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(dashDir * dashSpeed, 0f);

            dashTimer -= Time.fixedDeltaTime;
            isInvincible = true;

            if (dashTimer <= 0f)
            {
                dashState = DashState.Cooldown;
                dashTimer = dashCooldown;
                isDashing = false; 
                isInvincible = false;
                rb.gravityScale = originalGravity;
            }
        }
        else if (dashState == DashState.Cooldown)
        {
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
            {
                dashState = DashState.Ready;
            }
        }

    }

    // For ground check visualization
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
    
    // This helps prevent getting stuck on corners
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isGrounded && moveInput != 0)
        {
            // Check if we're stuck on a wall
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Check if normal is horizontal (meaning we're pushing against a wall)
                if (Mathf.Abs(contact.normal.x) > 0.9f)
                {
                    // Try to slightly adjust player's Y position up to get unstuck
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
                    break;
                }
            }
        }
    }

    public void ApplyMovementBuff(float speedMultiplier, float jumpMultiplier)
    {
        moveSpeed = baseMoveSpeed * speedMultiplier;
        jumpForce = baseJumpForce * jumpMultiplier;
    }

    public void ResetMovementStats()
    {
        moveSpeed = baseMoveSpeed;
        jumpForce = baseJumpForce;
    }

    public bool IsDashing()
    {
        return isDashing;
    }
    public bool IsInvincible()
    {
        return isInvincible;
    }

}