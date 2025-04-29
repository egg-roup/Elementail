using UnityEngine;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 9; // nine lives
    private int currentHearts;
    private HealthBar healthBar;
    private UIManager uiManager;

    void Start()
    {
        currentHearts = maxHearts;

        healthBar = FindFirstObjectByType<HealthBar>();
        uiManager = FindFirstObjectByType<UIManager>();

        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHearts);
        }
        else
        {
            Debug.LogError("No HealthBar found in the scene!");
        }
    }

    public void TakeDamage(int amount)
    {
        currentHearts -= amount;
        if (currentHearts < 0) currentHearts = 0;
        
        Debug.Log("Player Lives: " + currentHearts);

        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHearts); 
        }

        if (currentHearts <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        gameObject.SetActive(false);
        
        if (uiManager != null) {
            uiManager.ShowGameOver();
        }
    }
}
