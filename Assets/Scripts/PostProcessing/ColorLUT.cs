using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(ColorLUTRenderer), PostProcessEvent.AfterStack, "Custom/ColorLUT")]
public sealed class ColorLUT : PostProcessEffectSettings
{
    [Range(0f, 10f), Tooltip("Bloom effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 1f };
    
    public TextureParameter LUTTex = new TextureParameter();
}

public sealed class ColorLUTRenderer : PostProcessEffectRenderer<ColorLUT>
{
    RenderTexture gradedImage;
    public override void Init()
    {
        base.Init();
        gradedImage = RenderTexture.GetTemporary(Screen.width, Screen.height, 0, RenderTextureFormat.DefaultHDR);
    }

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Custom/PostEffects/ColorLUT"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetTexture("_LUTTex", settings.LUTTex);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    //    context.command.BlitFullscreenTriangle(gradedImage, context.destination);
    }
}