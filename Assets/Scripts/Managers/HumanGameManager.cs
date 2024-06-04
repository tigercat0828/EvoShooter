using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HumanGameManager : MonoBehaviour {


    public static HumanGameManager manager;
    public GameObject gameOverPanel;
    public GameObject gamePausePanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public SaveData data;


    private bool isPaused = false;
    private float previousTimeScale;

    private void Awake() {
        Globals.ResetScores();
        manager = this;
        SystemIO.Initialize();
        data = new SaveData(0);
        GameSettings.LoadSettings();

        gameOverPanel.SetActive(false);
        gamePausePanel.SetActive(false);
        Time.timeScale = 1f;

    }
    private void Start() {
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
        int score = Globals.GetScore(0);
        scoreText.text = $"Score: {Globals.GetScore(0)}";
        SystemIO.SaveHighscore(data, score);
        highScoreText.text = $"High Score : {data.highScore}";
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


