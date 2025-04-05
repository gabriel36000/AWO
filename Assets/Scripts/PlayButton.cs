using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour {

    public void PlayBtn()
    {
        SceneManager.LoadScene("Movement");
    }
    public void MainMenu() {
        SceneManager.LoadScene("LoginScreen");
    }
    public void Quit() {
        
           // UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
    }
}
