using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [Header("Combo Prefabs")]
    public GameObject sideswingPrefab;
    public GameObject upswingPrefab;
    public GameObject downswingPrefab;

    [Header("Damage Settings")]
    public float basicHitDamage = 1f;
    public float heavyHitDamage = 2f;

    private float baseBasicHitDamage;
    private float baseHeavyHitDamage;
    public float swordKnockbackForce;


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
    private Animator animator;

    void Start()
    {
        baseBasicHitDamage = basicHitDamage;
        baseHeavyHitDamage = heavyHitDamage;
        ApplySwordKnockback();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Left click = sword attack
        if (Input.GetButtonDown("SwordAttack") && !isAttacking && !isParrying && !playerController.IsDashing())
        {
            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick > comboResetTime)
                comboIndex = 0;

            lastClickTime = Time.time;

            switch (comboIndex)
            {
                case 0:
                    StartCoroutine(SpawnHitbox(sideswingPrefab, basicHitDamage));
                    break;
                case 1:
                    StartCoroutine(SpawnHitbox(upswingPrefab, basicHitDamage));
                    break;
                case 2:
                    StartCoroutine(SpawnHitbox(downswingPrefab, heavyHitDamage));
                    break;
            }

            comboIndex = (comboIndex + 1) % 3;
        }

        // Right click = parry
        if (Input.GetButtonDown("Parry") && !isParrying && !isAttacking && !isParryOnCooldown)
        {
            StartCoroutine(PerformParry());
        }
    }

    private System.Collections.IEnumerator SpawnHitbox(GameObject prefab, float damageMultiplier)
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        animator.SetTrigger("Attack");

        Quaternion rotation = playerController.facingRight ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);

        GameObject hitbox = Instantiate(prefab, hitboxSpawnPoint.position, rotation);
        hitbox.transform.localScale = new Vector3(6.5f, 1.5f, .5f); 

        SwordHitbox hitboxScript = hitbox.GetComponent<SwordHitbox>();
        if (hitboxScript != null)
            hitboxScript.damageMultiplier = damageMultiplier;
            hitboxScript.knockbackForce = swordKnockbackForce;

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
        parry.transform.localScale = parryHitboxPrefab.transform.localScale; 
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
    public void ApplySwordBuff(float basicMultiplier, float heavyMultiplier)
    {
        basicHitDamage = baseBasicHitDamage * basicMultiplier;
        heavyHitDamage = baseHeavyHitDamage * heavyMultiplier;
    }

    public void ResetSwordDamage()
    {
        basicHitDamage = baseBasicHitDamage;
        heavyHitDamage = baseHeavyHitDamage;
    }

    public void ApplySwordKnockback()
    {
        if (parryHitboxPrefab != null)
        {
            ParryHitbox parryScript = parryHitboxPrefab.GetComponent<ParryHitbox>();
            if (parryScript != null)
            {
                swordKnockbackForce = parryScript.parryKnockbackForce * 0.5f;
            }
            else
            {
                Debug.LogWarning("ParryHitbox component not found on parryHitboxPrefab.");
            }
        }
    }
}
