using UnityEngine;
using System.Collections;

// Interface to be implemented by any enemy that can be knocked back
public interface IKnockbackable 
{
    void ApplyKnockback(Vector2 force, float duration);
}

// Add this component to any enemy that needs knockback functionality
public class KnockbackController : MonoBehaviour, IKnockbackable
{
    private Rigidbody2D rb;
    private MonoBehaviour[] movementScripts;
    private bool isKnockedBack = false;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Get all movement behavior scripts
        movementScripts = new MonoBehaviour[] {
            GetComponent<EnemyGoomba>(),
            GetComponent<EnemyGhostFollow>(),
            GetComponent<EnemyJumpFollow>(),
            GetComponent<EnemySentry>()
        };
    }
    
    public void ApplyKnockback(Vector2 force, float duration)
    {
        if (!isKnockedBack && rb != null)
        {
            StartCoroutine(KnockbackRoutine(force, duration));
        }
    }
    
    private IEnumerator KnockbackRoutine(Vector2 force, float duration)
    {
        isKnockedBack = true;
        
        // Disable all movement scripts
        foreach (var script in movementScripts)
        {
            if (script != null)
                script.enabled = false;
        }
        
        // Apply the knockback force
        rb.linearVelocity = Vector2.zero; // Clear existing velocity
        rb.AddForce(force, ForceMode2D.Impulse);
        
        // Wait for the knockback duration
        yield return new WaitForSeconds(duration);
        
        // Re-enable all movement scripts that were active before
        foreach (var script in movementScripts)
        {
            if (script != null)
                script.enabled = true;
        }
        
        isKnockedBack = false;
    }
}