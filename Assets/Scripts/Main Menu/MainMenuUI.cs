using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
        SceneLoader.LoadScene(SceneName.LevelUntimedTest);
    }
    public void OnLevelSelectButtonPressed()
    {
        SceneLoader.LoadScene(SceneName.MenuModeSelect);
    }
    public void OnSettingsButtonPressed()
    {
        SceneLoader.LoadScene(SceneName.MenuSettings);
    }
}
