using UnityEngine;

public enum ElementType
{
    None,
    Fire,
    Water,
    Earth,
    Air,
    All
}

public class ElementRandomizer : MonoBehaviour
{
    public ElementType currentElement = ElementType.None;
    public ElementDisplayController elementDisplayController;

    public ElementType RollElement() 
    {
        float roll = Random.Range(0f, 100f);

        if (roll < 21f) return ElementType.Fire;
        else if (roll < 42f) return ElementType.Water;
        else if (roll < 63f) return ElementType.Earth;
        else if (roll < 84f) return ElementType.Air;
        else if (roll < 87f) return ElementType.All;
        else return ElementType.None;
    }

    public void DoRoll()
    {
        currentElement = RollElement();
        Debug.Log("Element Rolled: " + currentElement);
        
        if (elementDisplayController != null)
        {
            elementDisplayController.UpdateDisplay(currentElement);
        }

        // TODO: trigger toe glow / animations here
    }
}
