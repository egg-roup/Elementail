using UnityEngine;

public class PlayerStomp : MonoBehaviour
{
    public int stompDamage = 10;
    public float jumpForce = 15f;

    private Rigidbody2D playerRb;

    void Start()
    {
        playerRb = GetComponentInParent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Deal damage to the enemy
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(stompDamage);
            }

            // Bounce the player upward with full jump force
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jumpForce);
        }
    }
}
