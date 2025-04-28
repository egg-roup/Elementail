using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damageAmount = 15;
    public float damageCooldown = 1f;
    private float damageTimer = 0f;

    void Update()
    {
        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
    }

    private void TryDamage(GameObject target)
    {
        if (damageTimer <= 0f)
        {
            PlayerHealth hp = target.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damageAmount);
                damageTimer = damageCooldown;
            }
        }
    }

    private bool IsValidTarget(GameObject obj)
    {
        return obj.CompareTag("Player") && obj.name != "StompZone";
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsValidTarget(collision.gameObject))
            TryDamage(collision.gameObject);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (IsValidTarget(collision.gameObject))
            TryDamage(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsValidTarget(collision.gameObject))
            TryDamage(collision.gameObject);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (IsValidTarget(collision.gameObject))
            TryDamage(collision.gameObject);
    }
}
