using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Animations;

[Serializable]
[PostProcess(typeof(ScreenPainRenderer), PostProcessEvent.AfterStack, "Custom/ScreenPain")]
public sealed class ScreenPain : PostProcessEffectSettings
{
    [Range(1f, 20f), Tooltip("Pulse Speed.")]
    public FloatParameter pulseSpeed = new FloatParameter { value = 1f };
    [Range(0f, 1f), Tooltip("Pulse Effect.")]
    public FloatParameter painValue = new FloatParameter { value = 0f };
    [Range(0f, 1f), Tooltip("Pain Darkness.")]
    public FloatParameter painDarkness = new FloatParameter { value = 0f };
    public SplineParameter curve = new SplineParameter {};
}

public sealed class ScreenPainRenderer : PostProcessEffectRenderer<ScreenPain>
{
    RenderTexture gradedImage;
    public override void Init()
    {
        base.Init();
        gradedImage = RenderTexture.GetTemporary(Screen.width, Screen.height, 0, RenderTextureFormat.DefaultHDR);
    }

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Custom/PostEffects/PainPulse"));
        sheet.properties.SetFloat("_Effect", settings.curve.value.Evaluate(Mathf.Repeat(Time.time * settings.pulseSpeed, 10)/10));
        sheet.properties.SetFloat("_Strength", settings.painValue);
        sheet.properties.SetFloat("_Darkness", settings.painDarkness);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}