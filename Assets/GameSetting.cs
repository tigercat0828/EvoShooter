using UnityEngine;

public class GameSettingManager : MonoBehaviour{
    public static GameSettingManager Instance;
    public static SettingData options;
    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    public void LoadSettings() {
        
    }
    public void SaveSettings() {

    }
}