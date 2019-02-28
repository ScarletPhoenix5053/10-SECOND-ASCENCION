using UnityEngine;

public class LevelEndUI : MonoBehaviour
{
    public void OnRetstartButtonPressed()
    {
        SceneLoader.LoadScene(SceneName.LevelUntimedTest);
    }
    public void OnMenuButtonPressed()
    {
        SceneLoader.LoadScene(SceneName.MenuMain);
    }
}
