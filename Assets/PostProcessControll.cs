
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessControll : MonoBehaviour
{
    
    public ColorGrading colorGradingLayer = null;
    public SoundEffector soundEffector;


    [Header("Beep Settings")]
    [SerializeField][Range(0, 10)]
    private float expoValueDef = 0.38f;   // Current Value Exposure in ColorGrading
    [SerializeField][Range(0, 10)]
    private float expoBeep = 0.58f;       // Value Exposure for normal Beep in ColorGrading
    [SerializeField][Range(0, 10)]
    private float expoBeepBig = 2.58f;    // Value Exposure for Big Beep in ColorGrading

    [Space(20)]
    [SerializeField][Range(0,50)]
    private float beepOutSpeed = 0.1f;    // Flash increase speed
    [SerializeField][Range(0, 50)]
    private float beepInSpeed = 10f;      // Flash decrease speed

    private float k = 0;                  // Alpha for lerp
    [HideInInspector]
    public bool flash = false;            
    [HideInInspector]
    public bool flashBig = false;
    private int flashCount = 0;
    private bool flashForward = true;

    void Start()
    {
        PostProcessVolume volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out colorGradingLayer);
        expoValueDef = colorGradingLayer.postExposure.value;

    }

    private void Update()
    {
        BeepFlash();
    }

    private void BeepFlash()
    {
        if (flash && flashCount < 1)
        {
            if(flashForward)
            {
                k = k + (Time.deltaTime * beepInSpeed);
            }
            else
            {
                k = k + (Time.deltaTime * beepOutSpeed);
            }
            
            if (k > 1)
            {
                k = 0;
                if (flashForward)
                {
                    flashForward = false;
                }
                else
                {
                    flashForward = true;
                    flashCount++;
                }
            }

            if (flashForward)
            {
                if (!flashBig)
                {
                    colorGradingLayer.postExposure.value = Mathf.Lerp(expoValueDef, expoBeep, k);
                }
                else
                {
                    colorGradingLayer.postExposure.value = Mathf.Lerp(expoValueDef, expoBeepBig, k);
                }
            }
            else
            {
                if (!flashBig)
                {
                    colorGradingLayer.postExposure.value = Mathf.Lerp(expoBeep, expoValueDef, k);
                }
                else
                {
                    colorGradingLayer.postExposure.value = Mathf.Lerp(expoBeepBig, expoValueDef, k);
                }
            }
        }
        else
        {
            flashCount = 0;
            flash = false;
            flashBig = false;
        }
    }

    public void ChangeExposure(float Expose)
    {
        //print("ChangeExposure");
        colorGradingLayer.postExposure.value = Expose;
    }
    public void ReturnExposure()
    {
        colorGradingLayer.postExposure.value = expoValueDef;
    }  

    public IEnumerator BigBeep()
    {
        colorGradingLayer.postExposure.value = 1;
        yield return new WaitForSeconds(0.05f);
        colorGradingLayer.postExposure.value = expoValueDef;
       // yield return null;
    }

    public IEnumerator Beep()
    {
        //10
        soundEffector.PlayBeepSound();
        yield return new WaitForSeconds(1f);

        //9
        soundEffector.PlayBeepSound();
        flash = true;
        colorGradingLayer.postExposure.value = expoValueDef; 
        yield return new WaitForSeconds(1f - 0.05f);

        //8
        soundEffector.PlayBeepSound();
        flash = true;
        colorGradingLayer.postExposure.value = expoValueDef;
        yield return new WaitForSeconds(1f - 0.05f);

        //7
        soundEffector.PlayBeepSound();
        flash = true;
        colorGradingLayer.postExposure.value = expoValueDef;
        yield return new WaitForSeconds(1f - 0.05f);

        //6
        soundEffector.PlayBeepSound();
        flash = true;
        colorGradingLayer.postExposure.value = expoValueDef;
        yield return new WaitForSeconds(1f - 0.05f);

        //5
        soundEffector.PlayBeepSound();
        flash = true;
        colorGradingLayer.postExposure.value = expoValueDef;
        yield return new WaitForSeconds(1f - 0.05f);        
    }
}