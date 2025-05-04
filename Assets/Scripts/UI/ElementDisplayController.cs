using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class ElementDisplaySettings
{
    public ElementType elementType;
    public Sprite sprite;
    public Vector2 anchoredPosition;
    public Vector2 sizeDelta;
}

public class ElementDisplayController : MonoBehaviour
{
    public Image displayImage;
    public List<ElementDisplaySettings> elementSettings;

    public void UpdateDisplay(ElementType element)
    {
        foreach (var setting in elementSettings)
        {
            if (setting.elementType == element)
            {
                displayImage.sprite = setting.sprite;
                RectTransform rt = displayImage.rectTransform;
                rt.anchoredPosition = setting.anchoredPosition;
                rt.sizeDelta = setting.sizeDelta;
                return;
            }
        }
    }
}

