using UnityEngine;
using UnityEngine.UI;

public class ParryUIBar : MonoBehaviour
{
    public Image fillImage; 
    public float refillSpeed = 1f;

    private float targetFill = 1f;

    void Update()
    {
        if (fillImage == null)
        {
            Debug.LogWarning("Fill Image is not assigned!");
            return;
        }

        Debug.Log("Current fill: " + fillImage.fillAmount + ", Target: " + targetFill);
        fillImage.fillAmount = Mathf.MoveTowards(fillImage.fillAmount, targetFill, refillSpeed * Time.deltaTime);
    }

    public void OnParry()
    {
        Debug.Log("OnParry() called");
        targetFill = 0f;
    }

    public void ResetBar()
    {
        Debug.Log("ResetBar() called");
        targetFill = 1f;
    }
}

