using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public int TakeDamage(int amount)
    {
        Debug.Log("Enemy HP: " + currentHealth);
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();

        return amount;
    }

    void Die()
    {
        Debug.Log("Enemy Dead");
        Destroy(gameObject);
    }
}
