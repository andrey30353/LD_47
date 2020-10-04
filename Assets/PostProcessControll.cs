
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

    void Start()
    {
        PostProcessVolume volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out colorGradingLayer);
        exposureValueDef = colorGradingLayer.postExposure.value;

       // needValue = exposureValueDef;
        currentValue = exposureValueDef;
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
        colorGradingLayer.postExposure.value = needValue;
        yield return new WaitForSeconds(0.05f);
        colorGradingLayer.postExposure.value = exposureValueDef; 
        yield return new WaitForSeconds(1f - 0.05f);

        //8
        soundEffector.PlayBeepSound();
        colorGradingLayer.postExposure.value = needValue;
        yield return new WaitForSeconds(0.05f);
        colorGradingLayer.postExposure.value = exposureValueDef;
        yield return new WaitForSeconds(1f - 0.05f);

        //7
        soundEffector.PlayBeepSound();
        colorGradingLayer.postExposure.value = needValue;
        yield return new WaitForSeconds(0.05f);
        colorGradingLayer.postExposure.value = exposureValueDef;
        yield return new WaitForSeconds(1f - 0.05f);

        //6
        soundEffector.PlayBeepSound();
        colorGradingLayer.postExposure.value = needValue;
        yield return new WaitForSeconds(0.05f);
        colorGradingLayer.postExposure.value = exposureValueDef;
        yield return new WaitForSeconds(1f - 0.05f);

        //5
        soundEffector.PlayBeepSound();
        colorGradingLayer.postExposure.value = needValue;
        yield return new WaitForSeconds(0.05f);
        colorGradingLayer.postExposure.value = exposureValueDef;
        yield return new WaitForSeconds(1f - 0.05f);        
    }
}