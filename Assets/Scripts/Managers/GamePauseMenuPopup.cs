
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePauseMenuPopup : MonoBehaviour
{

    private bool isPaused = false;
    private float previousTimeScale;

    public GameObject gamePausePanel;
    // Start is called before the first frame update
    void Start()
    {
        gamePausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
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
    public void ChangeToMainMenuScene() {
        SceneManager.LoadScene("Menu");
    }
}
