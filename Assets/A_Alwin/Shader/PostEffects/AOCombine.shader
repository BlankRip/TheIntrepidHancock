Shader "Custom/PostEffects/AOCombine"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		TEXTURE2D_SAMPLER2D(_AOTexture, sampler_AOTexture);
		float _Strength, _Blend;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		float4 AO = SAMPLE_TEXTURE2D(_AOTexture, sampler_AOTexture, i.texcoord);
		
	//	return color * (1 - pow((1 - AO) * _Strength, 2));
		return color * lerp(1 - pow(AO, _Strength), 1, _Blend);
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