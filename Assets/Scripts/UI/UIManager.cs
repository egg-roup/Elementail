using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public AudioMixer audioMixer;

    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("UI References")]
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject gameOverScreen;
    public GameObject confirmQuitPanel; 
    public GameObject pauseButton;
    public GameObject endPanel;

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
        LoadVolume();
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        gameOverScreen.SetActive(false);
        confirmQuitPanel.SetActive(false);
        endPanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void OnEscapePressed()
    {
        if (gameOverScreen.activeSelf)
        {
            return; 
        }
        if (optionsMenu.activeSelf)
        {
            return;
        }
        if (endPanel.activeSelf)
        {
            pauseButton.SetActive(false);
            return;
        }
        else if (confirmQuitPanel.activeSelf) {
            CloseConfirmQuitPanel();
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

    public void ShowGameOver(){
        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
        pauseButton.SetActive(false);
        confirmQuitPanel.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        isPaused = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    public void QuitToMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenConfirmQuitPanel()
    {
        confirmQuitPanel.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverScreen.SetActive(false);
        Time.timeScale = 0f; 
    }

    public void CloseConfirmQuitPanel()
    {
        confirmQuitPanel.SetActive(false);
        pauseMenu.SetActive(true);
        if (!isPaused && !gameOverScreen.activeSelf)
        {
            Time.timeScale = 1f; 
        }
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();        
    }

    public bool IsPauseMenuOpen() => pauseMenu.activeSelf;
    public bool IsOptionsMenuOpen() => optionsMenu.activeSelf;

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume() 
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

}
