using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public GameObject healthIconPrefab;
    public Transform topRow;
    public Transform bottomRow;

    private List<GameObject> icons = new List<GameObject>();
    private int maxHearts = 9;
    private int currentHealth = 9;

    private HorizontalLayoutGroup topRowLayout;    // Layout group for top row
    private HorizontalLayoutGroup bottomRowLayout; // Layout group for bottom row

    void Start()
    {
        topRowLayout = topRow.GetComponent<HorizontalLayoutGroup>();
        bottomRowLayout = bottomRow.GetComponent<HorizontalLayoutGroup>();

        StartCoroutine(BuildHealthBarAfterFrame());
    }

    IEnumerator BuildHealthBarAfterFrame()
    {
        yield return null; // wait one frame
        BuildHealthBar();
    }

    void BuildHealthBar() {
        for (int i = 0; i < maxHearts; i++) {
            Transform parent = (i < 5) ? topRow : bottomRow;
            GameObject icon = Instantiate(healthIconPrefab, parent);
            icon.SetActive(true);
            icons.Add(icon);
        }
        
        // Update health display after icons are created
        UpdateHealth(currentHealth);
    }

    public void UpdateHealth(int currentHealth) {
        this.currentHealth = currentHealth;
        
        // If icons aren't built yet, just save the health value and return
        if (icons.Count == 0) {
            return;
        }
        
        topRowLayout.enabled = false;
        bottomRowLayout.enabled = false;

        for (int i = 0; i < icons.Count; i++) {
            if (i < currentHealth)
            {
                icons[i].SetActive(true);  // Heart is visible
            }
            else
            {
                icons[i].SetActive(false); // Heart is hidden (not removed, just invisible)
            }
        }

        topRowLayout.enabled = true;
        bottomRowLayout.enabled = true;
    }
}