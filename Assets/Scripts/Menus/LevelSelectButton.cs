using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    private LevelSelectUI ui;
    private int levelIndex;

    public void InitializeButton(LevelSelectUI levelSelectUI, int levelIndex)
    {
        ui = levelSelectUI;
        this.levelIndex = levelIndex;
        GetComponentInChildren<Text>().text = "Level " + (levelIndex + 1);
    }
    public void OnPress()
    {
        ui.LevelButtonPressed(levelIndex);
    }
}
