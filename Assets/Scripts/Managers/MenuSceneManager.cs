using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour {

    public void ChangeToHumanPlayScene() {
        SceneManager.LoadScene("HumanGame");
    }
    public void ChangeToOptionsScene() {
        SceneManager.LoadScene("Options");
    }
    public void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
