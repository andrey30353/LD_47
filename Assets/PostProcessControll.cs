
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessControll : MonoBehaviour
{
    public float exposureValueDef = 0.38f;
    public float bigBeepValue = 2.58f;
    private float currentValue = 0;
    public float needValue = 0.58f;
    public ColorGrading colorGradingLayer = null;
    public SoundEffector soundEffector;

    public bool flash = false;
    public bool flashBig = false;
    public float flashSpeed = 2f;
    public float flashBigSpeed = 2f;
    private int flashCount = 0;
    private bool flashForward = true;
    private float k = 0;

    void Start()
    {
        PostProcessVolume volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out colorGradingLayer);
        exposureValueDef = colorGradingLayer.postExposure.value;

       // needValue = exposureValueDef;
        currentValue = exposureValueDef;
    }

    private void Update()
    {
        FlashStart(flashSpeed);
    }

    private void FlashStart(float speed)
    {
        if (flash && flashCount < 1)
        {
            k = k + (Time.deltaTime * speed);
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
                    colorGradingLayer.postExposure.value = Mathf.Lerp(0.38f, 0.58f, k);
                }
                else
                {
                    colorGradingLayer.postExposure.value = Mathf.Lerp(0.38f, 2.58f, k);
                }

            }
            else
            {
                if (!flashBig)
                {
                    colorGradingLayer.postExposure.value = Mathf.Lerp(0.58f, 0.38f, k);
                }
                else
                {
                    colorGradingLayer.postExposure.value = Mathf.Lerp(2.58f, 0.58f, k);
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
        colorGradingLayer.postExposure.value = exposureValueDef;
    }  

    public IEnumerator BigBeep()
    {
        colorGradingLayer.postExposure.value = 1;
        yield return new WaitForSeconds(0.05f);
        colorGradingLayer.postExposure.value = exposureValueDef;
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
        colorGradingLayer.postExposure.value = exposureValueDef; 
        yield return new WaitForSeconds(1f - 0.05f);

        //8
        soundEffector.PlayBeepSound();
        flash = true;
        colorGradingLayer.postExposure.value = exposureValueDef;
        yield return new WaitForSeconds(1f - 0.05f);

        //7
        soundEffector.PlayBeepSound();
        flash = true;
        colorGradingLayer.postExposure.value = exposureValueDef;
        yield return new WaitForSeconds(1f - 0.05f);

        //6
        soundEffector.PlayBeepSound();
        flash = true;
        colorGradingLayer.postExposure.value = exposureValueDef;
        yield return new WaitForSeconds(1f - 0.05f);

        //5
        soundEffector.PlayBeepSound();
        flash = true;
        colorGradingLayer.postExposure.value = exposureValueDef;
        yield return new WaitForSeconds(1f - 0.05f);        
    }
}