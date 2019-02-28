using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;

    #region Unity Messages
    private void Awake()
    {
        CheckIfSingleton();
        
        LevelManager.OnLevelEnd += OnLevelEnd;
        SceneManager.sceneLoaded += OnSceneLoad;
    }
    #endregion

    #region Public Methods
    #endregion
    #region Private Methods
    private void CheckIfSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        // Check if in a playable level
        if (scene.name == "LevelUntimedTest" ||
            scene.name == "LevelHighscore")
        {
            LevelManager.StartLevel();
        }
    }
    private void OnLevelEnd(int score)
    {
        LevelManager.OnLevelEnd -= OnLevelEnd;

        Debug.Log("Exited level with a score of: " + score);

        SceneLoader.LoadScene(SceneName.MenuGameOver);
    }
    #endregion
}
