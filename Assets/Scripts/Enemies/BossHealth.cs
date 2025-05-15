using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private bool isDead = false;
    private bool hasBeenTriggered = false;

    public BossBattleManager battleManager;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"{gameObject.name} started with {maxHealth} health.");

        if (battleManager == null)
        {
            Debug.LogWarning($"{gameObject.name} has no battleManager assigned!");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        if (!hasBeenTriggered)
        {
            battleManager.TriggerBoss(this);
            hasBeenTriggered = true;
        }

    currentHealth -= damage;
    Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {currentHealth}");

    if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (battleManager != null)
        {
            battleManager.NotifyBossDeath(this);
        }

        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
