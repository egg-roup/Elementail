using UnityEngine;

public class ElementBuffApplier : MonoBehaviour
{
    public ElementRandomizer elementRandomizer;

    public PlayerController playerController;
    public SwordAttack swordAttack;
    public ParryHitbox parryHitbox;
    public PlayerStomp playerStomp;
    public PlayerHealth playerHealth;

    private ElementType lastElement = ElementType.None;

    void Update()
    {
        if (elementRandomizer.currentElement != lastElement)
        {
            ApplyElementBuff(elementRandomizer.currentElement);
            lastElement = elementRandomizer.currentElement;
        }
    }

    void ApplyElementBuff(ElementType element)
    {
        // Reset all stats first
        playerController?.ResetMovementStats();
        swordAttack?.ResetSwordDamage();
        parryHitbox?.ResetParryStats();
        playerStomp?.ResetStompStats();

        switch (element)
        {
            case ElementType.Air:
                playerController?.ApplyMovementBuff(1.35f, 1.35f); // +speed, +jump
                parryHitbox?.ApplyParryBuff(parryHitbox.parryDamage, parryHitbox.parryKnockbackForce * 1.75f); // +knockback
                break;

            case ElementType.Earth:
                playerStomp?.SetStompStats(4, playerStomp.jumpForce); // 1-shot + strong rebound
                break;

            case ElementType.Water:
                // Heal 2, there are health mechanics already in PlayerHealth
                playerHealth.Heal(2); 
                parryHitbox?.ApplyParryBuff(parryHitbox.parryDamage + 1, parryHitbox.parryKnockbackForce); // +parry damage
                break;

            case ElementType.Fire:
                swordAttack?.ApplySwordBuff(2f, 2f); // 1,1,2 → 2,2,4
                break;

            case ElementType.All:
                playerController?.ApplyMovementBuff(1.35f, 1.35f);
                swordAttack?.ApplySwordBuff(2f, 2f);
                parryHitbox?.ApplyParryBuff(parryHitbox.parryDamage + 1, parryHitbox.parryKnockbackForce * 1.75f);
                playerStomp?.SetStompStats(4, playerStomp.jumpForce);
                playerHealth.Heal(2); 
                break;

            case ElementType.None:
                // Nothing to apply
                break;
        }

        Debug.Log("Element buffs applied: " + element);
    }

    private bool playerHealthHasRoom()
    {
        return playerHealth != null && playerHealth.GetCurrentHealth() < playerHealth.GetMaxHealth();
    }
}
