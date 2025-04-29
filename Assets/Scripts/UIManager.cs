using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI References")]
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject pauseButton;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void OnEscapePressed()
    {
        if (optionsMenu.activeSelf)
        {
            CloseOptionsMenu();
        }
        else if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame() {
        Debug.Log("PauseGame() called");
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        pauseButton.SetActive(false);
        isPaused = true;
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseButton.SetActive(true);
        isPaused = false;
    }

    public void OpenOptionsMenu() {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseOptionsMenu() {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();        
    }

    public bool IsPauseMenuOpen() => pauseMenu.activeSelf;
    public bool IsOptionsMenuOpen() => optionsMenu.activeSelf;

}
