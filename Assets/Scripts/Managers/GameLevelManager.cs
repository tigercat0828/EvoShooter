using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelManager : MonoBehaviour {

    public static GameLevelManager manager;
    public GameObject gameOverPanel;
    public GameObject gamePausePanel;
    public TextMeshProUGUI scoreText;
    public SaveData data;


    private bool isPaused = false;
    private float previousTimeScale;

    private void Awake() {
        manager = this;
     
        
        //SystemIO.Initialize();
        //data = new SaveData(0);

        gameOverPanel.SetActive(false);
        gamePausePanel.SetActive(false);
        GameSettings.LoadSettings();
        Time.timeScale = 1f;
    }
    private void Start() {
        Globals.instance.ResetAllStatus();
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
    public void GameOver() {

        gameOverPanel.SetActive(true);
        scoreText.text = $"Score: {Globals.instance.GetScore(0)}";
    }
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


