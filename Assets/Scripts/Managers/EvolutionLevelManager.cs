using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class EvolutionLevelManager : MonoBehaviour {

    private const int MAX_SLOTS_NUM =2;
    public static EvolutionLevelManager manager;
    public GameObject gamePausePanel;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI GenerationText;


    private bool isPaused = false;
    private float previousTimeScale;


    public GameObject AgentPrefab;
    public Transform[] EntityDish = new Transform[MAX_SLOTS_NUM];

    private void Awake() {
        manager = this;
       
        GameSettings.LoadSettings();
        gamePausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    private void Start() {
        Globals.intance.ResetAllStatus();
        SpawnAgents();
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
    public void SpawnAgents() {
        for (int i = 0; i < EntityDish.Length; i++) {
            Agent agent = Instantiate(AgentPrefab, EntityDish[i].position, Quaternion.identity, EntityDish[i]).GetComponent<Agent>();
            agent.SetSlot(i);
            agent.IsInEvoScene = true;
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


