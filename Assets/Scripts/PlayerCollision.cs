using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            // TODO: Handle HP loss
            Debug.Log("Player touched damage tile (solid). Lose HP here.");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            // Optional: continuous damage if standing on spikes
            Debug.Log("Player standing on damage tile (solid). Lose HP over time here.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DamagePass"))
        {
            // Handle fire tile damage (pass-through)
            Debug.Log("Player passing through damage pass tile. Lose HP here.");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DamagePass"))
        {
            // Continuous fire damage
            Debug.Log("Player inside damage pass tile. Lose HP repeatedly.");
        }
    }
}
