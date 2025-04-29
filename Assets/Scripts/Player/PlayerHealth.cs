using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 9; // nine lives
    private int currentHearts;
    private HealthBar healthBar;
    private UIManager uiManager;
    private Animator animator; 
    public bool canControl = true;


    void Start()
    {
        currentHearts = maxHearts;

        healthBar = FindFirstObjectByType<HealthBar>();
        uiManager = FindFirstObjectByType<UIManager>();
        animator = GetComponent<Animator>();

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
        
        if (animator != null)
        {
            Debug.Log("Trigger Hurt");
            animator.SetTrigger("Hurt"); 
        }

        if (currentHearts <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");

        GetComponent<PlayerController>().enabled = false;

        if (animator != null)
        {
            animator.SetBool("isDead", true);
            animator.SetTrigger("Die");
        }

        StartCoroutine(WaitForDeathAnimation());
    }

    IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(1.5f); // Adjust to match your death animation length

        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }
    }

}
