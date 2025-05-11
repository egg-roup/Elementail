using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public int baseDamage = 1;
    public float duration = 0.2f;
    public float damageMultiplier = 1f;
    public float knockbackForce = 0f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();
            if (health != null)
            {
                int totalDamage = Mathf.RoundToInt(baseDamage * damageMultiplier);
                health.TakeDamage(totalDamage);
            }
            KnockbackController knockback = other.GetComponent<KnockbackController>() ?? other.gameObject.AddComponent<KnockbackController>();
            Vector2 direction = (other.transform.position - transform.position).normalized;
            knockback.ApplyKnockback(direction * knockbackForce, 0.3f);
        }
    }
}
