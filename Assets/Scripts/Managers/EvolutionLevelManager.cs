using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EvolutionLevelManager : MonoBehaviour {

    public static EvolutionLevelManager manager;
    public GameObject gamePausePanel;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI GenerationText;


    private bool isPaused = false;
    private float previousTimeScale;

    private void Awake() {
        Globals.ResetAllStatus();
        manager = this;
        GameSettings.LoadSettings();
        gamePausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Update() {
        // click ESC so can pause the game
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                ResumeGame();
            }
            else {
                PauseGame();
            }
        }
    }



    // Scene Control
    // ==============================================================================================================
    public void ReplayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void PauseGame() {
        isPaused = true;
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        gamePausePanel.SetActive(true); 
    }
    public void ResumeGame() {
        isPaused = false;
        Time.timeScale = previousTimeScale; 
        gamePausePanel.SetActive(false); 
    }
    public void ChangeToMenuScene() {
        SceneManager.LoadScene("Menu");
    }

}


