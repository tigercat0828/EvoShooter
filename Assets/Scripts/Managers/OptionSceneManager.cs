using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionSceneManager : MonoBehaviour {
    // Start is called before the first frame update
    public void ChangeToMenuScene() {
        SceneManager.LoadScene("Menu");
    }
    
    public void ApplySettings() {
        
    }
    public void LoadSettings() {
         
    }
}

