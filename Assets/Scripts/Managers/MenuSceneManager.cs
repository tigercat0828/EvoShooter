using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour {

    public void ChangeToHumanPlayScene() {
        SceneManager.LoadScene("HumanGame");
    }
    public void ChangeToOptionsScene() {
        SceneManager.LoadScene("Options");
    }
    public void ChangeToEvolutionScene() {
        SceneManager.LoadScene("EvolutionGame");
    }
    public void ChangeToAgentPlayScene() {
        SceneManager.LoadScene("AgentGame");
    }
    public void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
