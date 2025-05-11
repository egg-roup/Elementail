using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [Header("Combo Prefabs")]
    public GameObject sideswingPrefab;
    public GameObject upswingPrefab;
    public GameObject downswingPrefab;

    [Header("Parry Prefab")]
    public GameObject parryHitboxPrefab;

    [Header("References")]
    public Transform hitboxSpawnPoint;
    public PlayerController playerController;
    public ParryUIBar parryUIBar; 

    private int comboIndex = 0;
    private float lastClickTime = -999f;
    private float comboResetTime = 2f;

    private bool isAttacking = false;
    private bool isParrying = false;
    private float attackCooldown = 0.3f;
    private bool isParryOnCooldown = false;
    private bool parrySuccess = false;

    void Update()
    {
        // Left click = sword attack
        if (Input.GetMouseButtonDown(0) && !isAttacking && !isParrying)
        {
            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick > comboResetTime)
                comboIndex = 0;

            lastClickTime = Time.time;

            switch (comboIndex)
            {
                case 0:
                    StartCoroutine(SpawnHitbox(sideswingPrefab, 1f));
                    break;
                case 1:
                    StartCoroutine(SpawnHitbox(upswingPrefab, 1f));
                    break;
                case 2:
                    StartCoroutine(SpawnHitbox(downswingPrefab, 2f));
                    break;
            }

            comboIndex = (comboIndex + 1) % 3;
        }

        // Right click = parry
        if (Input.GetMouseButtonDown(1) && !isParrying && !isAttacking && !isParryOnCooldown)
        {
            StartCoroutine(PerformParry());
        }
    }

    private System.Collections.IEnumerator SpawnHitbox(GameObject prefab, float damageMultiplier)
    {
        isAttacking = true;

        Quaternion rotation = playerController.facingRight ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);

        GameObject hitbox = Instantiate(prefab, hitboxSpawnPoint.position, rotation);

        SwordHitbox hitboxScript = hitbox.GetComponent<SwordHitbox>();
        if (hitboxScript != null)
            hitboxScript.damageMultiplier = damageMultiplier;

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
    private System.Collections.IEnumerator PerformParry()
    {
        isParrying = true;
        isParryOnCooldown = true;
        parrySuccess = false;

        Quaternion rotation = playerController.facingRight ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);
        GameObject parry = Instantiate(parryHitboxPrefab, hitboxSpawnPoint.position, rotation, transform);
        ParryHitbox hitboxScript = parry.GetComponent<ParryHitbox>();
        if (hitboxScript != null)
        {
            hitboxScript.parryUIBar = parryUIBar;
        }
        
        yield return new WaitForSeconds(0.2f);
        Destroy(parry);
        isParrying = false;

        float cooldown = parrySuccess ? 0.8f : 1.8f;

   

        yield return new WaitForSeconds(cooldown);
        isParryOnCooldown = false;
    }

    public void OnSuccessfulParry()
    {
        parrySuccess = true;
    }

    public bool IsParrying()
    {
        return isParrying;
    }
}
