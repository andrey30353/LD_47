using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckStrawberry : MonoBehaviour
{

    public int strawberryAmountOnLevel;
    public int currentStrawberryAmount;
    public Image WinScreen;
    public Button Pause;


   void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Strawberry"))
        {
            currentStrawberryAmount++;

            if (currentStrawberryAmount == strawberryAmountOnLevel)
            {
                Time.timeScale = 0f;
                Pause.gameObject.SetActive(false);
                WinScreen.gameObject.SetActive(true);
            }

        }
    }
}
