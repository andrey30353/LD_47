using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessControll : MonoBehaviour
{

    private float exposureValueDef = 0.38f;
    private ColorGrading colorGradingLayer = null;

    void Start()
    {
        PostProcessVolume volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out colorGradingLayer);
        exposureValueDef = colorGradingLayer.postExposure.value;
    }

    public void ChangeExposure(float Expose)
    {
        colorGradingLayer.postExposure.value = Expose;
    }
    public void ReturnExposure()
    {
        colorGradingLayer.postExposure.value = exposureValueDef;
    }
}
