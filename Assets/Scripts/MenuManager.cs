using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public void ChangeToHumanPlayScene() {
        SceneManager.LoadScene("HumanGame");
    }
    public void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
