using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HumanPlaySceneManager : MonoBehaviour {

    public enum GameStates {
        Pause, Running, End
    }
    public static HumanPlaySceneManager manager;
    public GameObject gameOverScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public int score;
    public GameStates GameState;
    public SaveData data;
    private void Awake() {
        manager = this;
        SystemIO.Initialize();
        data = new SaveData(0);
        score = 0;
        GameSettings.LoadSettings();

        //gameOverScreen.SetActive(false);

    }
    private void Start() {
        GameState = GameStates.Running;
    }
    public void GameOver() {

        gameOverScreen.SetActive(true);
        scoreText.text = $"Score: {score}";
        SystemIO.SaveHighscore(data, score);
        highScoreText.text = $"High Score : {data.highScore}";
    }
    public void ReplayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ChangeToMenuScene() {
        SceneManager.LoadScene("Menu");
    }
    public void IncreaseScore(int amount) {
        score += amount;
    }

}


