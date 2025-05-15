using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicMap : MonoBehaviour
{
    [System.Serializable]
    public struct SceneMusic
    {
        public string sceneName;
        public string trackName;
    }

    public SceneMusic[] sceneMusicList;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var sm in sceneMusicList)
        {
            if (sm.sceneName == scene.name)
            {
                MusicManager.Instance.PlayMusic(sm.trackName);
                break;
            }
        }
    }
}
