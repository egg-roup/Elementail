using UnityEngine;

public class PlayerStomp : MonoBehaviour
{
    public int stompDamage = 10;
    public float jumpForce = 15f;

    private Rigidbody2D playerRb;
    private SwordAttack swordAttack; // Reference to SwordAttack component

    void Start()
    {
        playerRb = GetComponentInParent<Rigidbody2D>();
        swordAttack = GetComponentInParent<SwordAttack>(); // Get reference to SwordAttack
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if we're currently parrying - if so, don't activate stomp
        if (swordAttack != null && swordAttack.IsParrying())
            return;
            
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