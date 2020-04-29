Shader "Custom/PostEffects/PainPulse"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		float _Effect, _Strength, _Darkness;
		float4 _PainColor;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		// test gray scale
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
		float3 mulColor = lerp(1, float3(1,0,0), _Effect * _Strength);
		color.rgb = lerp(color.rgb, luminance * _Darkness, _Strength) * mulColor;
		return color;
	}

		ENDHLSL

		SubShader
	{
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment Frag

			ENDHLSL
		}
	}
}