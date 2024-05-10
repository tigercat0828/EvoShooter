using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HumanPlayLevelManager : MonoBehaviour
{

    public enum GameStates {
        Pause, Running, End
    }
    public static HumanPlayLevelManager manager;
    public GameObject gameOverScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public int score;
    public GameStates GameState;
    public SaveData data;
    private void Awake() {
        manager = this;
        SaveSystem.Initialize();
        data = new SaveData(0);
        gameOverScreen.SetActive(false);
        
    }
    private void Start() {
        GameState = GameStates.Running;
    }
    public void GameOver() {

        gameOverScreen.SetActive(true);
        scoreText.text = $"Score: {score}";
        string loadedData = SaveSystem.Load("highscore");
        if(loadedData != null) {
            data = JsonUtility.FromJson<SaveData>(loadedData);
        }
        if(score > data.highScore) {
            data.highScore = score;
            string saveData = JsonUtility.ToJson(data);
            SaveSystem.Save("highscore", saveData);
        }
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


