using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsButton : MonoBehaviour
{
    public Button[] buttons;
    
    
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++) 
        {
            if (PlayerPrefs.GetInt("LvlsPassed") >= i) 
            {
                buttons[i].interactable = true;
            }
            else 
            {
                buttons[i].interactable = false;
            }
        }
    }

    void Update()
    {
        
    }
}
