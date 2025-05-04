using UnityEngine;
using UnityEngine.UI;

public class ElementCooldown : MonoBehaviour
{
    public Image palmFill;
    public float cooldownDuration = 60f;
    public ElementRandomizer elementRandomizer;

    private float cooldownTimer = 0f;
    private bool isCoolingDown = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isCoolingDown) {
            RollAndStartCooldown();
        }

        if (isCoolingDown)
        {
            cooldownTimer -= Time.deltaTime;
            float fillAmount = Mathf.Clamp01(cooldownTimer / cooldownDuration);
            palmFill.fillAmount = fillAmount;

            if (cooldownTimer <= 0f)
            {
                isCoolingDown = false;
                Debug.Log("Cooldown done!");

                palmFill.fillAmount = 0f;
                // TODO: trigger paw blink or reroll available state
            }
        }
    }

    void RollAndStartCooldown()
    {
        if (elementRandomizer != null)
        {
            elementRandomizer.DoRoll();
        }

        StartCooldown();
    }

    public void StartCooldown()
    {
        palmFill.gameObject.SetActive(true);
        cooldownTimer = cooldownDuration;
        isCoolingDown = true;
        palmFill.fillAmount = 1f;
    }
}
