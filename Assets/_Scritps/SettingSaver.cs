using System.IO;
using UnityEngine;

[System.Serializable]
public class SettingData
{
    public float masterVolume = 1.0f;
    public int resolutionIndex = 0;
    public bool isFullscreen = true;
    public int qualityIndex = 2;
    public int difficulty = 1;
}

public class SettingSaver : MonoBehaviour
{
    private static string filePath = Application.persistentDataPath + "/settings.json";
    public static SettingData currentSetting;

    private void Awake()
    {
        LoadSettings();
    }

    public static void SaveSettings()
    {
        string json = JsonUtility.ToJson(currentSetting, true);
        File.WriteAllText(filePath, json);
    }

    private static void LoadSettings()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            currentSetting = JsonUtility.FromJson<SettingData>(json);
            return;
        }

        currentSetting = new SettingData();
        SaveSettings();
    }
}
