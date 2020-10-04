using System.Collections;
using UnityEngine;

public class StrawberryAnim : MonoBehaviour
{
    public GameObject[] parts;

    public void Eat()
    {
        StartCoroutine(StartAnim());
    }


    IEnumerator StartAnim()
    {
        for (int i = 1; i < parts.Length; i++)
        {
            parts[i-1].SetActive(false);
            parts[i].SetActive(true);

            yield return new WaitForSeconds(0.1f);
        }        
    }
}
