using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ParryHitbox : MonoBehaviour
{
    public GameObject successVFXPrefab;
    public float knockbackDuration = 0.5f;
    private SwordAttack swordAttack;
    private Animator animator;

    public ParryUIBar parryUIBar;
    
    // Track which objects we've already parried
    private HashSet<Collider2D> parriedObjects = new HashSet<Collider2D>();

    void Start()
    {
        swordAttack = GetComponentInParent<SwordAttack>();
        animator = transform.root.GetComponent<Animator>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Parry hitbox triggered with: " + other.name);

        // If we've already parried this object, ignore it
        if (parriedObjects.Contains(other))
            return;

        if (other.CompareTag("Enemy") || other.CompareTag("EnemyProjectile"))
        {
            // Add to our already-parried set
            parriedObjects.Add(other);

            // Handle projectile parry
            if (other.CompareTag("EnemyProjectile"))
            {
                Destroy(other.gameObject);
            }

            // Apply knockback to enemies
            if (other.CompareTag("Enemy"))
            {
                // Get enemy components
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                EnemyDamage enemyDamage = other.GetComponent<EnemyDamage>();
                
                // Get the knockback controller (or add one if missing)
                KnockbackController knockback = other.GetComponent<KnockbackController>();
                if (knockback == null)
                {
                    knockback = other.gameObject.AddComponent<KnockbackController>();
                }

                if (enemyHealth != null)
                {
                    // Use the enemy's own damage value if available
                    int damage = enemyDamage != null ? enemyDamage.damageAmount : 10;
                    int actualDamage = enemyHealth.TakeDamage(damage);
                    
                    // Calculate knockback - with no multiplier as requested
                    Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
                    float knockbackForce = actualDamage * 5; 
                    
                    // Apply knockback using our interface
                    knockback.ApplyKnockback(knockbackDir * knockbackForce, knockbackDuration);
                    
                    Debug.Log($"Applied knockback to {other.name}: Direction {knockbackDir}, Force {knockbackForce}");
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

            if (animator != null)
            {
                animator.SetTrigger("Parry"); 
            }

            // Notify parry success
            swordAttack?.OnSuccessfulParry();
        }

        if (parryUIBar != null)
        {
            Debug.Log("Parry bar triggered!");
            parryUIBar.OnParry();
            StartCoroutine(RefillParryBar(1f));
        }
    }

    private IEnumerator RefillParryBar(float delay)
    {
        yield return new WaitForSeconds(delay);
        parryUIBar?.ResetBar();
    }
}