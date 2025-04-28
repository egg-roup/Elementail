using UnityEngine;

public class ParryHitbox : MonoBehaviour
{
    public GameObject successVFXPrefab;
    private SwordAttack swordAttack;

    void Start()
    {
        swordAttack = GetComponentInParent<SwordAttack>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Parry hitbox triggered with: " + other.name);
        
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyProjectile"))
        {

            // Handle projectile parry
            if (other.CompareTag("EnemyProjectile"))
            {
                Destroy(other.gameObject); // Optional: add reflect logic
            }

            // Apply knockback to enemies
            if (other.CompareTag("Enemy"))
            {
                Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
                    float knockbackForce = 7f; // You can tweak this
                    enemyRb.linearVelocity = new Vector2(knockbackDir.x * knockbackForce, enemyRb.linearVelocity.y);
                }
            }

            // Spawn VFX
            if (successVFXPrefab == null)
                Debug.LogWarning("No parry VFX prefab assigned!");
            if (successVFXPrefab != null)
            {
                Instantiate(successVFXPrefab, transform.position, Quaternion.identity);
                Debug.Log("Parry flash spawned!");
            }

            // Notify parry success
            swordAttack?.OnSuccessfulParry();
        }
    }

}
