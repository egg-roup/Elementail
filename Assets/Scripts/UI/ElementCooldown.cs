using UnityEngine;
using UnityEngine.UI;

public class ElementCooldown : MonoBehaviour
{
    public Image palmFill;
    public float cooldownDuration = 60f;
    public ElementRandomizer elementRandomizer;
    public ElementDisplayController elementDisplayController;

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
                if (elementDisplayController != null)
                {
                    elementDisplayController.displayImage.gameObject.SetActive(false);  
                }
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

        if (elementDisplayController != null)
        {
            elementDisplayController.displayImage.gameObject.SetActive(true);  
        }
    }
}
