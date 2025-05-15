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
        Debug.Log("SceneMusicMap enabled and listening for scene load events.");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("SceneMusicMap disabled and no longer listening for scene load events.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");

        bool musicFound = false; 

        foreach (var sm in sceneMusicList)
        {
            if (sm.sceneName == scene.name)
            {
                Debug.Log($"Playing music for scene '{scene.name}': {sm.trackName}");
                MusicManager.Instance.PlayMusic(sm.trackName);
                break;
            }
        }

        if (!musicFound)
        {
            Debug.LogWarning($"No music track mapped for scene: {scene.name}");
        }
    }
}
