using System.IO;
using UnityEngine;

[System.Serializable]
public class SettingData
{
    public float masterVolume = 1.0f;
    public int qualityIndex = 2;
    public int resolutionIndex = -1;
    public int difficulty = 0;
    public bool fullscreen = true;
}

public class SettingSaver : MonoBehaviour
{
    private static string filePath;
    public static SettingData currentSetting;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/settings.json";
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
