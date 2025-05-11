using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
    public float coyoteTime = 0.1f; // Time player can jump after leaving ground
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;
    
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
        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            coyoteTimeCounter = 0; // Reset coyote time when jumping
        }
        
        // Variable jump height - if player releases jump button, reduce upward velocity
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // Update Animator Parameters
        anim.SetFloat("Speed", Mathf.Abs(moveInput));
        anim.SetBool("IsGrounded", isGrounded);

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
}