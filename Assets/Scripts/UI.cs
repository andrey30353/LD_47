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


        if (!PlayerPrefs.HasKey("LvlsPassed")) 
        {
            PlayerPrefs.SetInt("LvlsPassed", 0);
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


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void OpenLevel1() 
    {
        SceneManager.LoadScene(1);
    }

    public void OpenLevel2()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenLevel3()
    {
        SceneManager.LoadScene(1);
    }




}
