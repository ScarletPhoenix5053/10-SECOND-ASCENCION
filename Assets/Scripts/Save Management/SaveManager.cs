using UnityEngine;

public static class SaveManager
{
    public static void SaveSetting(string label, float value)
    {
        PlayerPrefs.SetFloat(label, value);
    }
    public static float LoadSetting(string label)
    {
        return PlayerPrefs.GetFloat(label);
    }
}
