Shader "Custom/PostEffects/SimpleSoble"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	//	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		sampler2D _CameraDepthTexture;
		float _Blend;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float factor = 0.714;

		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		// depth main
		float depth = tex2D(_CameraDepthTexture, i.texcoord).r;
		// depth up
		float depthU = tex2D(_CameraDepthTexture, i.texcoord + float2(0, _Blend)).r;
		// depth up right
		float depthUR = tex2D(_CameraDepthTexture, i.texcoord + float2(_Blend, _Blend) * factor).r;
		// depth up left
		float depthUL = tex2D(_CameraDepthTexture, i.texcoord + float2(-_Blend, _Blend) * factor).r;
		// depth down
		float depthD = tex2D(_CameraDepthTexture, i.texcoord + float2(0, -_Blend)).r;
		// depth down right
		float depthDR = tex2D(_CameraDepthTexture, i.texcoord + float2(_Blend, -_Blend) * factor).r;
		// depth down left
		float depthDL = tex2D(_CameraDepthTexture, i.texcoord + float2(-_Blend, -_Blend) * factor).r;
		// depth right
		float depthR = tex2D(_CameraDepthTexture, i.texcoord + float2(_Blend, 0)).r;
		// depth up
		float depthL = tex2D(_CameraDepthTexture, i.texcoord + float2(-_Blend, 0)).r;
		
	//	float darkValue = (step(depthL, depth) + step(depthR, depth) + step(depthU, depth) + step(depthD, depth)) / 4;
		float darkValue = max(depthU - depth, 0) + max(depthD - depth, 0) + max(depthR - depth, 0) + max(depthL - depth, 0) + max(depthUR - depth, 0) + max(depthUL - depth, 0) + max(depthDR - depth, 0) + max(depthDL - depth, 0);
		
		return color * (1 - darkValue * 500);
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