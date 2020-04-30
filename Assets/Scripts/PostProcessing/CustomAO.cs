﻿using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(AORenderer), PostProcessEvent.BeforeTransparent, "Custom/CustomAO")]
public sealed class CustomAO : PostProcessEffectSettings
{
    [Range(0f, 2f), Tooltip("Bloom effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };
    [Range(0f, 20f), Tooltip("Bloom effect intensity.")]
    public FloatParameter sampleShift = new FloatParameter { value = 0.5f };
    [Range(0f, 5f), Tooltip("Bloom effect intensity.")]
    public FloatParameter blurCount = new FloatParameter { value = 0.5f };
    [Range(0f, 0.1f), Tooltip("Bloom effect intensity.")]
    public FloatParameter shift = new FloatParameter { value = 0.5f };
    [Range(0f, 5f), Tooltip("Bloom effect intensity.")]
    public FloatParameter maxRange = new FloatParameter { value = 0.5f };
    [Range(0f, 100f), Tooltip("Bloom effect intensity.")]
    public FloatParameter scanDistance = new FloatParameter { value = 0.5f };
     [Range(0f, 100f), Tooltip("Bloom effect intensity.")]
    public FloatParameter strength = new FloatParameter { value = 0.5f };
     [Range(0f, 1f), Tooltip("Bloom effect intensity.")]
    public FloatParameter effect = new FloatParameter { value = 0.5f };
     [Range(0f, 5f), Tooltip("Height Shift.")]
    public FloatParameter heightShift = new FloatParameter { value = 0.5f };
     [Range(0f, 5f), Tooltip("Height Shift.")]
    public BoolParameter debug = new BoolParameter { value = false };

    //  [Range(0f, 1f), Tooltip("Bloom effect cutoff.")]
    //  public FloatParameter cutoff = new FloatParameter { value = 0.5f };
    //  [Range(0f, 0.1f), Tooltip("Bloom fine control.")]
    //   public FloatParameter shift = new FloatParameter { value = 0.5f };
    //    [Range(0f, 20f), Tooltip("Bloom loop.")]
    //  public IntParameter bloomCount = new IntParameter { value = 2 };
}

public sealed class AORenderer : PostProcessEffectRenderer<CustomAO>
{


    // bloom extract meatrial

    RenderTexture rt1, rt2;
    // bloom extract meatrial

    public override void Init()
    {
        base.Init();
        rt1 = RenderTexture.GetTemporary(Screen.width / 2, Screen.height / 2);
        rt2 = RenderTexture.GetTemporary(Screen.width / 2, Screen.height / 2);


    }
    public override void Render(PostProcessRenderContext context)
    {
        var aoSheet = context.propertySheets.Get(Shader.Find("Custom/PostEffects/SSAO"));
        Matrix4x4 camToWorld = Camera.current.cameraToWorldMatrix;
     //   aoSheet.properties.SetMatrix("_CamToWorld", Camera.current.cameraToWorldMatrix);
        aoSheet.properties.SetFloat("_Blend", settings.blend);
        aoSheet.properties.SetFloat("_SampleShift", settings.sampleShift);
        aoSheet.properties.SetFloat("_ScanDistance", settings.scanDistance);
        aoSheet.properties.SetFloat("_MaxRange", settings.maxRange);
        aoSheet.properties.SetFloat("_HeightFactor", settings.heightShift);
        aoSheet.properties.SetMatrix("_InverseProjectionMatrix",
                           GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, false)
                               .inverse);

        aoSheet.properties.SetMatrix("_ProjectionMatrix",
                           GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, false)
                               );
        // bluring section

        var blurVertical = context.propertySheets.Get(Shader.Find("Custom/PostEffects/BlurVertical"));
        var blurHorizontal = context.propertySheets.Get(Shader.Find("Custom/PostEffects/BlurHorizontal"));

        context.command.BlitFullscreenTriangle(context.source, rt1, aoSheet, 0);
        
        if(!settings.debug){
        
        // blur
        for (int i = 0; i < settings.blurCount; i++)
        {

            // blur vertical
            
            blurVertical.properties.SetFloat("_Shift", settings.shift);
            context.command.BlitFullscreenTriangle(rt1, rt2, blurVertical, 0);

            // blur horizontal
            
            blurHorizontal.properties.SetFloat("_Shift", settings.shift);
            context.command.BlitFullscreenTriangle(rt2, rt1, blurHorizontal, 0);

        }
        
        // clean it
     //   context.command.BlitFullscreenTriangle(rt1, rt2, blurHorizontal, 0);
        
         // bloom texture extraction
    //     var extractor = context.propertySheets.Get(Shader.Find("Custom/PostEffects/Highlight"));
     //    extractor.properties.SetFloat("_Cutoff", settings.cutoff);
         

        var mergeSheet = context.propertySheets.Get(Shader.Find("Custom/PostEffects/AOCombine"));
        mergeSheet.properties.SetFloat("_Strength", settings.strength);
        mergeSheet.properties.SetTexture("_AOTexture", rt1); 
        mergeSheet.properties.SetFloat("_Effect", settings.effect);
        context.command.BlitFullscreenTriangle(context.source, context.destination, mergeSheet, 0);
        }

        else context.command.BlitFullscreenTriangle(rt1, context.destination);
    }
}