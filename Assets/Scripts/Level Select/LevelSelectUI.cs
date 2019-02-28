using UnityEngine;
using System.Collections;

public class LevelSelectUI : MonoBehaviour
{
    #region Public Variables
    public int LevelCatalogLength = 0;
    public int FurthestLevelIndex = 0;

    public LevelSelectButton ButtonPrefab;
    public LevelSelectButton LockedButtonPrefab;
    public Transform ButtonContainer;
    #endregion

    #region Unity Messages
    public void Start()
    {
        GenerateLevelButtons();
    }
    #endregion

    #region Public Methods
    public void OnMainMenuButtonPressed()
    {
        SceneLoader.LoadScene(SceneName.MenuMain);
    }
    public void GenerateLevelButtons()
    {
        for (int i = 0; i < LevelCatalogLength; i++)
        {
            if (i < FurthestLevelIndex)
            {
                InstantiateButton(ButtonPrefab, i);
            }
            else
            {
                InstantiateButton(LockedButtonPrefab, i);
            }
        }
    }
    public void LevelButtonPressed(int levelIndex)
    {
        // change scene
        SceneLoader.LoadScene(SceneName.LevelUntimedTest);
    }
    #endregion
    #region Private Methods
    /// <summary>
    /// Instantiate and initialize a button inside <see cref="ButtonContainer"/>
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="index"></param>
    private void InstantiateButton(LevelSelectButton prefab, int index)
    {
        LevelSelectButton newButton = Instantiate(prefab, ButtonContainer);
        newButton.InitializeButton(this, index);
    }
    #endregion
}
