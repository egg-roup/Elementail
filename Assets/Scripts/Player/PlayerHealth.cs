using UnityEngine;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 9; // nine lives
    private int currentHearts;

    private HealthBar healthBar;

    void Start()
    {
        currentHearts = maxHearts;
        healthBar = FindObjectOfType<HealthBar>(); 
        healthBar.UpdateHealth(currentHearts);
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
        // Do more game over stuff here
    }
}
