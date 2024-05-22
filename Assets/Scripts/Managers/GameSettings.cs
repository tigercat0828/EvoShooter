using UnityEngine;

public static class GameSettings {
    public static SettingData options = new();
    public readonly static string SETTINGS_FILENAME = "GamesSettings";
    public static void LoadSettings() {
        if (PlayerPrefs.HasKey(SETTINGS_FILENAME)) {
            string json = PlayerPrefs.GetString(SETTINGS_FILENAME);
            options = JsonUtility.FromJson<SettingData>(json);
        }
        else {
            options = new();
        }
    }
    public static void SaveSettings() {
        string json = JsonUtility.ToJson(options);
        PlayerPrefs.SetString(SETTINGS_FILENAME, json);
        PlayerPrefs.Save();
    }
}