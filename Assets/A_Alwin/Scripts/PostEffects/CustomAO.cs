using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(AORenderer), PostProcessEvent.AfterStack, "Custom/CustomAO")]
public sealed class CustomAO : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Bloom effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };
    [Range(0f, 5f), Tooltip("Bloom effect intensity.")]
    public FloatParameter cutoff = new FloatParameter { value = 0.5f };

    public TextureParameter screenNormal = new TextureParameter();

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

    public override void Init()
    {
        base.Init();
    }
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Custom/PostEffects/AO"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetFloat("_BackCutoff", settings.cutoff);
        sheet.properties.SetTexture("_ScreenNoise", settings.screenNormal);
        /*
         // bloom texture extraction
         var extractor = context.propertySheets.Get(Shader.Find("Custom/PostEffects/Highlight"));
         extractor.properties.SetFloat("_Cutoff", settings.cutoff);
         */
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}