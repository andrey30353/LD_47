using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckStrawberry : MonoBehaviour
{       
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Strawberry"))
        {
            var anim = other.GetComponent<StrawberryAnim>();
           
            if (anim != null)
            {
                anim.IsEaten = true;
                //anim.Eat();
            }

            

        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Strawberry"))
        {
            var anim = other.GetComponent<StrawberryAnim>();

            if (anim != null)
            {
                anim.IsEaten = true;
                //anim.Eat();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Strawberry"))
        {
            var anim = other.GetComponent<StrawberryAnim>();

            if (anim != null)
            {
                anim.IsEaten = false;
                //anim.Eat();
            }
        }
    }
}
