using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep InputManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapePress();
        }
    }

    private void HandleEscapePress()
    {
        if (UIManager.Instance == null)
        {
            Debug.LogWarning("UIManager not found!");
            return;
        }

        UIManager.Instance.OnEscapePressed();

        var menuController = FindObjectOfType<MainMenuController>();
        if (menuController != null)
        {
            if (menuController.optionsPanel.activeSelf)
                menuController.CloseOptions();
            else if (menuController.aboutPanel.activeSelf)
                menuController.CloseAbout();
        }
    }

}

