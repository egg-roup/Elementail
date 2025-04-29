using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject aboutPanel;

    void Start()
    {
        ShowMainPanel();
    }

    public void StartGame() {
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenOptions() {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OpenAbout() {
        mainPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void CloseAbout()
    {
        aboutPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

        public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        aboutPanel.SetActive(false);
    }

}
