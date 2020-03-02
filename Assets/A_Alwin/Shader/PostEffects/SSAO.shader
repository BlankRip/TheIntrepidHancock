Shader "Custom/PostEffects/SSAO"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
//#include "Support.cginc"
			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		//	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	//	sampler2D _MainTex;
		sampler2D _CameraDepthTexture;
		sampler2D _CameraDepthNormalsTexture;
		float _Blend;
		float _BackCutoff;
		float _ScanDistance;

		float random(float2 st) {
			return frac(sin(dot(st.xy , float2(12.9898, 78.233)))*43758.5453123);
		}
		/*
		float3 RandomRay(float2 screenPos)
		{
			screenPos *= 200;
			float3 screenNormal = float3(random(screenPos + float2(0.7, 0.1)), random(screenPos + float2(0.2, 0.5)), random(screenPos + float2(0.1, 0.7)));
			screenNormal = (screenNormal * 2) - 1;
			screenNormal = mul(unity_MatrixVP, screenNormal);
			return screenNormal;
		}
		*/
		float3 RandomRay(float2 screenPos, float factor)
		{
			screenPos *= 2000.73;
			float3 screenNormal = float3(random(screenPos + float2(0.7, 0.1)), random(screenPos + float2(0.2, 0.5)), random(screenPos + float2(0.1, 0.7)));
			screenNormal = normalize((screenNormal * 2) - 1) * factor;
			screenNormal = mul(unity_MatrixVP, screenNormal);
			return screenNormal;
		}

		float AmbiantFactor(float2 screenPos) 
		{
			float pointDepth = tex2D(_CameraDepthTexture, screenPos).r;
			pointDepth = LinearEyeDepth(pointDepth);
			float factor = 0;
			for (float i = 1; i < 64; i++)
			{
				// make a random direction vector
				half3 randomDir = normalize(RandomRay(screenPos + float2(0.01 * i, 0), _ScanDistance * (i/64)));
				// get the screen Vector point
				float randomPointDepth = tex2D(_CameraDepthTexture, screenPos + (randomDir.xy * _Blend)).r;
				randomPointDepth = LinearEyeDepth(randomPointDepth);
				float effect = _BackCutoff * max(randomPointDepth - pointDepth, 0);

				factor += step(pointDepth, randomPointDepth) * 1/ (i/ 32);
		//		factor += 1 * (1 - step( pointDepth - randomPointDepth, _BackCutoff));
				// cutoff back distance check
			//	factor += (1 - step(pointDepth - randomPointDepth, _BackCutoff));
				//factor
			}
			return factor/64;
		}
		


	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

		return 1 - saturate(AmbiantFactor(i.texcoord));
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