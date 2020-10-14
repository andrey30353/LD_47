using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StrawberryAnim : MonoBehaviour
{
    public GameObject[] parts;

    public float EatTime;

    public float partTime => EatTime / parts.Length;


    public bool IsEaten;
    private float eatProgress;

    public SoundEffector soundEffector;

    /*
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

            yield return new WaitForSeconds(0.3f);
        }

        Game.Instance.currentStrawberryAmount++;

        if (Game.Instance.currentStrawberryAmount == Game.Instance.StrawberyCount)
        {
            //Game.Instance.CurrentWorm.enabled = false;
            Time.timeScale = 0f;

            Game.Instance.Pause.gameObject.SetActive(false);
            Game.Instance.WinScreen.gameObject.SetActive(true);
        }
    }*/


    int curPart = 1;
    private void Update()
    {
        if (!IsEaten)
            return;

        eatProgress += Time.deltaTime;

        if(eatProgress > partTime * curPart)
        {
            parts[curPart - 1].SetActive(false);
            parts[curPart].SetActive(true);

            curPart++;

            soundEffector.PlayCrawlSound();
        }

        if(curPart == parts.Length)
        {
            Game.Instance.currentStrawberryAmount++;

            //print($"cur = { Game.Instance.currentStrawberryAmount} need = {Game.Instance.StrawberyCount}");
            if (Game.Instance.currentStrawberryAmount == Game.Instance.StrawberyCount)
            {
                //Game.Instance.CurrentWorm.enabled = false;
                Time.timeScale = 0f;

                Game.Instance.Pause.gameObject.SetActive(false);
                Game.Instance.WinScreen.gameObject.SetActive(true);

                if (PlayerPrefs.HasKey("LvlsPassed")) 
                {
                    if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("LvlsPassed"))
                    {
                        PlayerPrefs.SetInt("LvlsPassed", SceneManager.GetActiveScene().buildIndex);
                    }
                }
                else 
                {
                    PlayerPrefs.SetInt("LvlsPassed", SceneManager.GetActiveScene().buildIndex);
                }

               
            }

            enabled = false;
        }
    }
}
