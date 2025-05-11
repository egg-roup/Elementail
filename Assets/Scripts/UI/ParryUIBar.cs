using UnityEngine;
using UnityEngine.UI;

public class ParryUIBar : MonoBehaviour
{
    public Image fillImage;

    private float targetFill = 1f;
    private float refillSpeed = 1f; 

    void Start()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = 1f;
        }
    }

    void Update()
    {
        if (fillImage == null) return;

        // Smoothly refill over time
        fillImage.fillAmount = Mathf.MoveTowards(fillImage.fillAmount, targetFill, refillSpeed * Time.deltaTime);
    }

    public void OnParryFail()
    {
        if (fillImage == null) return;

        fillImage.fillAmount = 0f;
        targetFill = 1f;
        refillSpeed = 1f / 2f; 
    }

    public void OnParrySuccess()
    {
        if (fillImage == null) return;

        fillImage.fillAmount = 0.5f;
        targetFill = 1f;
        refillSpeed = 0.5f / 1f; 
    }

    public void ResetBar()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = 1f;
            targetFill = 1f;
        }
    }
}
