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
            var anim = other.GetComponent<StrawberryAnim>();
            if(anim != null)
            {
                anim.Eat();
            }

            currentStrawberryAmount++;

            if (currentStrawberryAmount == Game.Instance.StrawberyCount)
            {
                Time.timeScale = 0f;
                Game.Instance.Pause.gameObject.SetActive(false);
                Game.Instance.WinScreen.gameObject.SetActive(true);
            }

        }
    }
}
