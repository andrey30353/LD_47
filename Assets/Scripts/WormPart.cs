using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WormPartType
{
    Head,
    Mid, 
    Tail
}

public class WormPart : MonoBehaviour
{    
    public Worm worm;

    public WormPartType Type;
   
    private void OnTriggerEnter(Collider other)
    {       
        if (Type != WormPartType.Head)
            return;

        var strawberry = other.GetComponent<StrawberryAnim>();
        if (strawberry != null)
            strawberry.IsEaten = worm.enabled;     
    }

    private void OnTriggerStay(Collider other)
    {       
        if (Type != WormPartType.Head)
            return;

        var strawberry = other.GetComponent<StrawberryAnim>();
        if (strawberry != null)
            strawberry.IsEaten = worm.enabled;
    }

    private void OnTriggerExit(Collider other)
    {        

        if (Type != WormPartType.Head)
            return;

        var strawberry = other.GetComponent<StrawberryAnim>();
        if (strawberry != null)
            strawberry.IsEaten = false;
    }
    

}
