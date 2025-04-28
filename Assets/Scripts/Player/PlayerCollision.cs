using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public float damageCooldown = 2f; // seconds between damage
    private float damageTimer = 0f;

    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (damageTimer > 0f)
            damageTimer -= Time.deltaTime;
    }

    private void TryDamage(int amount)
    {
        if (damageTimer <= 0f)
        {
            playerHealth.TakeDamage(amount);
            damageTimer = damageCooldown;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            TryDamage(1); // spikes
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            TryDamage(1); // spikes
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DamagePass"))
        {
            TryDamage(1); // fire
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DamagePass"))
        {
            TryDamage(1); // fire
        }
    }
}
