using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    
    public void Exit()
    {
        Application.Quit();
    }

    public void OpenMainScene()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }


    public void Continue()
    {
        Time.timeScale = 1f;
    }


}
