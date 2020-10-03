using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    
    public void Exit()
    {
        Application.Quit();
    }

    public void OpenScene()
    {
        SceneManager.LoadScene(1);
    }

}
