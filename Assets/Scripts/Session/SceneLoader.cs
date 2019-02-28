using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadScene(SceneName name)
    {
        SceneManager.LoadScene((int)name);
    }
}
public enum SceneName
{
    MenuMain = 0,
    MenuModeSelect = 1,
    MenuGameOver = 2,
    MenuSettings = 3,
    LevelUntimedTest = 4,
    LevelHighscore = 5
}