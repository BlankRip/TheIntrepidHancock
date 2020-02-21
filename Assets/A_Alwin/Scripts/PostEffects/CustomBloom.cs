using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(BloomRenderer), PostProcessEvent.AfterStack, "Custom/CustomBloom")]
public sealed class CustomBloom : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Bloom effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };
    [Range(0f, 1f), Tooltip("Bloom effect cutoff.")]
    public FloatParameter cutoff = new FloatParameter { value = 0.5f };
    [Range(0f, 0.1f), Tooltip("Bloom fine control.")]
    public FloatParameter shift = new FloatParameter { value = 0.5f };
    [Range(0f, 20f), Tooltip("Bloom loop.")]
    public IntParameter bloomCount = new IntParameter { value = 2 };
}

public sealed class BloomRenderer : PostProcessEffectRenderer<CustomBloom>
{

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
        var sheet = context.propertySheets.Get(Shader.Find("Custom/PostEffects/AddTextures"));
        sheet.properties.SetFloat("_Blend", settings.blend);

       
        // bloom texture extraction
        var extractor = context.propertySheets.Get(Shader.Find("Custom/PostEffects/Highlight"));
        extractor.properties.SetFloat("_Cutoff", settings.cutoff);

        context.command.BlitFullscreenTriangle(context.source, rt1, extractor, 0);

        

        // blur
        for (int i = 0; i < settings.bloomCount; i++)
        {
            
        // blur vertical
        var blurVertical = context.propertySheets.Get(Shader.Find("Custom/PostEffects/BlurVertical"));
        blurVertical.properties.SetFloat("_Shift", settings.shift);
        context.command.BlitFullscreenTriangle(rt1, rt2, blurVertical, 0);

        // blur horizontal
        var blurHorizontal = context.propertySheets.Get(Shader.Find("Custom/PostEffects/BlurHorizontal"));
        blurHorizontal.properties.SetFloat("_Shift", settings.shift);
        context.command.BlitFullscreenTriangle(rt2, rt1, blurHorizontal, 0);

        }
        

        sheet.properties.SetTexture("_MainTexB", rt1);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}