using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HumanGameManager : MonoBehaviour {


    public static HumanGameManager manager;
    public GameObject gameOverPanel;
    public GameObject gamePausePanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public int score;
    public SaveData data;


    private bool isPaused = false;
    private float previousTimeScale;

    private void Awake() {
        manager = this;
        SystemIO.Initialize();
        data = new SaveData(0);
        score = 0;
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
        scoreText.text = $"Score: {score}";
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
    public void IncreaseScore(int amount) {
        score += amount;
    }
    public void DecreaseScore(int amount) {
        score -= amount; 
    }

}


