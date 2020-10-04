using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffector : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip crawlSound;
    public AudioClip eatSound;
    public AudioClip beepSound;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCrawlSound()
    {
        audioSource.PlayOneShot(crawlSound);
    }

     public void PlayEatSound()
    {
        audioSource.PlayOneShot(eatSound);
    }

    public void PlayBeepSound()
    {
        audioSource.PlayOneShot(beepSound);
    }


}
