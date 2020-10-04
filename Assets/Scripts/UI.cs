using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public void Start()
    {
        Time.timeScale = 1f;

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            PlayerPrefs.SetInt("ContinueLevel", 1);
        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            PlayerPrefs.SetInt("ContinueLevel", 2);
        }

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            PlayerPrefs.SetInt("ContinueLevel", 3);
        }
    }
    
    public void Exit()
    {
        Application.Quit();
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

    public void OpenLevel1()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenLevel2()
    {
        SceneManager.LoadScene(2);
    }

    public void OpenLevel3()
    {
        SceneManager.LoadScene(3);
    }

    public void ContinueLevel()
    {
        if (PlayerPrefs.GetInt("ContinueLevel") == 1)
        {
            SceneManager.LoadScene(1);
        }

        if (PlayerPrefs.GetInt("ContinueLevel") == 2)
        {
            SceneManager.LoadScene(2);
        }

        if (PlayerPrefs.GetInt("ContinueLevel") == 3)
        {
            SceneManager.LoadScene(3);
        }
    }


}
