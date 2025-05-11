using UnityEngine;

public class PlayerStomp : MonoBehaviour
{
    public int stompDamage = 1;
    public float jumpForce = 15f;

    private int baseStompDamage;
    private float baseJumpForce;
    private Rigidbody2D playerRb;
    private SwordAttack swordAttack; 

    void Start()
    {
        playerRb = GetComponentInParent<Rigidbody2D>();
        swordAttack = GetComponentInParent<SwordAttack>(); 
        baseStompDamage = stompDamage;
        baseJumpForce = jumpForce;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (swordAttack != null && swordAttack.IsParrying())
            return;

        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(stompDamage);
            }

            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jumpForce);
        }
    }
    public void SetStompStats(int newDamage, float newJumpForce)
    {
        stompDamage = newDamage;
        jumpForce = newJumpForce;
    }

    public void ResetStompStats()
    {
        stompDamage = baseStompDamage;
        jumpForce = baseJumpForce;
    }
}