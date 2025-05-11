using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParryHitbox : MonoBehaviour
{
    public GameObject successVFXPrefab;
    public float knockbackDuration = 0.5f;
    public ParryUIBar parryUIBar;

    private SwordAttack swordAttack;
    private Animator animator;
    private bool triggeredSuccess = false;
    private HashSet<Collider2D> parriedObjects = new HashSet<Collider2D>();
    public int parryDamage = 2;
    public float parryKnockbackForce = 5f;

    private int baseParryDamage;
    private float baseParryKnockbackForce;

    void Start()
    {
        swordAttack = GetComponentInParent<SwordAttack>();
        animator = transform.root.GetComponent<Animator>();
        StartCoroutine(ParryFailTimeout());

        baseParryDamage = parryDamage;
        baseParryKnockbackForce = parryKnockbackForce;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (parriedObjects.Contains(other) || triggeredSuccess)
            return;

        if (other.CompareTag("Enemy") || other.CompareTag("EnemyProjectile"))
        {
            triggeredSuccess = true;
            parriedObjects.Add(other);

            if (other.CompareTag("EnemyProjectile"))
            {
                Destroy(other.gameObject);
            }

        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            KnockbackController knockback = other.GetComponent<KnockbackController>() ?? other.gameObject.AddComponent<KnockbackController>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(parryDamage);
                Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
                knockback.ApplyKnockback(knockbackDir * parryKnockbackForce, knockbackDuration);
            }
        }

            if (successVFXPrefab != null)
                Instantiate(successVFXPrefab, transform.position, Quaternion.identity);

            animator?.SetTrigger("Parry");
            swordAttack?.OnSuccessfulParry();
            parryUIBar?.OnParrySuccess();
        }
    }

    private IEnumerator ParryFailTimeout()
    {
        yield return new WaitForSeconds(0.2f);
        if (!triggeredSuccess)
        {
            parryUIBar?.OnParryFail();
        }
    }
    public void ApplyParryBuff(int newDamage, float newKnockback)
    {
        parryDamage = newDamage;
        parryKnockbackForce = newKnockback;
    }

    public void ResetParryStats()
    {
        parryDamage = baseParryDamage;
        parryKnockbackForce = baseParryKnockbackForce;
    }

}
