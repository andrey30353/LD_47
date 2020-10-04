using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffector : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip crawlSound;
    public AudioClip eatSound;


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


}
