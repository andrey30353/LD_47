using UnityEngine;

public class CheckStrawberry : MonoBehaviour
{
    private Snake snake;

    private void Awake()
    {
        snake = GetComponentInParent<Snake>();
    }

    void OnTriggerEnter(Collider other)
    {       
        if (other.CompareTag("Strawberry"))
        {
            var anim = other.GetComponent<StrawberryAnim>();
           
            if (anim != null)
            {
                anim.IsEaten = snake.enabled;                
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
                anim.IsEaten = snake.enabled;
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
            }
        }
    }
}
