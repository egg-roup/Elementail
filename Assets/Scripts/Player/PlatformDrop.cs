using UnityEngine;

public class PlatformDrop : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float dropResetTime = 0.25f;
    public float controllerDownThreshold = -0.5f;  

    private void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {

        bool keyboardDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
        bool controllerDown = Input.GetAxis("Vertical") < controllerDownThreshold;

        if (keyboardDown || controllerDown)
        {
            StartCoroutine(DropThrough());
        }
    }

    private System.Collections.IEnumerator DropThrough()
    {
        effector.rotationalOffset = 180f;
        yield return new WaitForSeconds(dropResetTime);
        effector.rotationalOffset = 0f;
    }
}
