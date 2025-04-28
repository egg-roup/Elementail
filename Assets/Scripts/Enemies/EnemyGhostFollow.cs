using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyGhostFollow : MonoBehaviour
{
    public float speed = 2f;
    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }
}
