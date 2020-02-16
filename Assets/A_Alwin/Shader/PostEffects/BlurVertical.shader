Shader "Custom/PostEffects/BlurVertical"
{
	HLSLINCLUDE

		#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		float _Shift;

		float4 VerticalBlur(VaryingsDefault i) : SV_Target
		{
			float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord) * 0.16;
			color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0, 2.0 * _Shift)) * 0.25;
			color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0, 3.0 * _Shift)) * 0.05;
			color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0, 4.0 * _Shift)) * 0.09;
			color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0, 5.0 * _Shift)) * 0.12;

			color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0, -2.0 * _Shift)) * 0.25;
			color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0, -3.0 * _Shift)) * 0.05;
			color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0, -4.0 * _Shift)) * 0.09;
			color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0, -5.0 * _Shift)) * 0.12;
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
				#pragma fragment VerticalBlur

			ENDHLSL
		}
	}
}