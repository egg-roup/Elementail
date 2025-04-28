using UnityEngine;

public class EnemySentry : MonoBehaviour
{
    public float detectionRange = 5f;
    public float maxChaseDuration = 5f;

    private Transform player;
    private Vector2 startPosition;

    private bool isChasing = false;
    private bool returning = false;

    private MonoBehaviour movementScript;
    private float chaseTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;

        // Get movement behavior
        movementScript = GetComponent<EnemyGhostFollow>() as MonoBehaviour;
        if (movementScript == null)
            movementScript = GetComponent<EnemyJumpFollow>() as MonoBehaviour;

        if (movementScript == null)
            Debug.LogWarning("No compatible movement script on Sentry!");

        if (movementScript != null)
            movementScript.enabled = false;
    }

    void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (distToPlayer <= detectionRange)
        {
            if (!isChasing)
            {
                isChasing = true;
                returning = false;
                movementScript.enabled = true;
                chaseTimer = 0f;
            }

            chaseTimer += Time.deltaTime;

            if (chaseTimer >= maxChaseDuration)
            {
                StopChasingAndReturn();
            }
        }
        else
        {
            if (isChasing)
            {
                StopChasingAndReturn();
            }
        }

        if (returning)
        {
            ReturnToStart();
        }
    }

    void StopChasingAndReturn()
    {
        isChasing = false;
        returning = true;
        chaseTimer = 0f;
        if (movementScript != null)
            movementScript.enabled = false;
    }

    void ReturnToStart()
    {
        Vector2 direction = (startPosition - (Vector2)transform.position).normalized;
        float distance = Vector2.Distance(transform.position, startPosition);

        transform.position += (Vector3)(direction * 2f * Time.deltaTime);

        if (distance < 0.1f)
        {
            returning = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
