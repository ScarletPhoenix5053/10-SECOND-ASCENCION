using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    public Slider Music, Sfx;

    private const string MusicPref = "MusicLevel";
    private const string SFXPref = "SFXLevel";

    private void Start()
    {
        LoadSettings();
    }
    
    public void LoadSettings()
    {
        // IMPORTANT! values are inverted when saved/loaded so that on first load the sliders default to 1, not 0.
        Music.value = 1f - SaveManager.LoadSetting(MusicPref);
        Sfx.value = 1f - SaveManager.LoadSetting(SFXPref);
    }
    public void SaveSettings()
    {
        // IMPORTANT! values are inverted when saved/loaded so that on first load the sliders default to 1, not 0.
        SaveManager.SaveSetting(MusicPref, 1f - Music.value);
        SaveManager.SaveSetting(SFXPref, 1f - Sfx.value);

        SceneLoader.LoadScene(SceneName.MenuMain);
    }
    public void ReturnToMainMenu()
    {
        SceneLoader.LoadScene(SceneName.MenuMain);
    }
}
