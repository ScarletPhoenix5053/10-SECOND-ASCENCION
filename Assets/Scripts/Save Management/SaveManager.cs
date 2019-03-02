using UnityEngine;

public static class SaveManager
{
    public static void SaveSetting(string label, float value)
    {
        PlayerPrefs.SetFloat(label, value);
    }
    /// <summary>
    /// Saves the passed score, if the new score is higher than any saved score
    /// </summary>
    /// <param name="label"></param>
    /// <param name="score"></param>
    public static void SaveScore(float score)
    {

    }
    public static float LoadSetting(string label)
    {
        return PlayerPrefs.GetFloat(label);
    }
}
