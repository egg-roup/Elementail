using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyGoomba : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveDirection = Vector2.left;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    // Optional: flip direction on collision with wall or edge
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Turn around if hitting a wall or enemy
        if (!collision.collider.CompareTag("Player") && collision.contacts.Length > 0)
        {
            moveDirection = -moveDirection;
            FlipSprite();
        }
    }

    void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Optional: public method to kill Goomba
    public void Die()
    {
        // Play death animation/sound here if needed
        Destroy(gameObject);
    }
}
