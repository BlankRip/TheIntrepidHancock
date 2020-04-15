Shader "Custom/PostEffects/SSAO_Chorence"
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
			return frac(sin(dot(st.xy , float2(121.9891, 785.237)))*4132517.5453123);
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
			screenPos *= 2;
			float3 screenNormal = float3(random(screenPos + float2(0.71, -0.1)), random(screenPos + float2(-0.25, -0.5)), random(screenPos + float2(0.17, 0.7)));
			screenNormal = normalize((screenNormal * 2) - 1) * factor ;
			screenNormal = mul(unity_MatrixVP, screenNormal);
			return screenNormal;
		}

		float AmbiantFactor(float2 screenPos) 
		{
			float screenDepth = tex2D(_CameraDepthTexture, screenPos).r;
			float pointDepth = LinearEyeDepth(screenDepth);
			float factor = 0;
			half3 randomDir;
			/*
			for (float i = 1; i < 64; i++)
			{
				// make a random direction vector
			//	half3 randomDir = normalize(RandomRay(screenPos + float2(0.01 * i, 0), _ScanDistance * (i/64)));
			//	half3 randomDir = RandomRay(screenPos + float2(0.01 * i, 0), _ScanDistance * (i / 64) * screenDepth);
				randomDir = RandomRay(screenPos + float2(0.01 * i, 0), (i / 64) * (1 - screenDepth));
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
			
			float3 shiftPoint_R = tex2D(_CameraDepthTexture, screenPos + float2(-0.05 * (screenDepth) * 10, 0.0)).r;
			float3 shiftPoint_L = tex2D(_CameraDepthTexture, screenPos + float2(0.05 * (screenDepth) * 10, 0.0)).r;
			float3 shiftPoint_U = tex2D(_CameraDepthTexture, screenPos + float2(0.0, 0.05 * (screenDepth) * 10)).r;
			float3 shiftPoint_D = tex2D(_CameraDepthTexture, screenPos + float2(0.0, -0.05 * (screenDepth) * 10)).r;
			*/
			for (float i = 1; i < 64; i++)
			{
				float3 screenRay = RandomRay(screenPos + float2(sin(i), cos(i + 8.7)), (1 - (i/64))) ;
				float value = tex2D(_CameraDepthTexture, screenPos + (screenRay.xy * _ScanDistance * (screenDepth) * _BackCutoff)).r;

			//	factor += max(value - screenDepth, 0) * 10 * (i/64);
				factor += max(value - screenDepth, 0) * (i/64);
			}


		//	return 1 - (factor/64);
	//	return 1 - screenDepth * 10;
	//	return float4(randomDir, 1);
//	return float4(float3(random(screenPos + float2(0.71, -0.1)), 0, 0), 1);
		return 1 - (factor/64) * 500;
/*
		return 1 - (
			max(shiftPoint_R - screenDepth, 0) + 
			max(shiftPoint_L - screenDepth, 0) + 
			max(shiftPoint_U - screenDepth, 0) +
			max(shiftPoint_D - screenDepth, 0)) * 100;
			*/
		}
		


	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

	//	return (1 - saturate(AmbiantFactor(i.texcoord)));
	//	return float4(RandomRay(i.texcoord, 1), 1);
	return AmbiantFactor(i.texcoord);
		//float pointDepth = tex2D(_CameraDepthTexture, i.texcoord).r;
	//	return LinearEyeDepth(pointDepth);
	//	return color;
	//	return (saturate(AmbiantFactor(i.texcoord)));
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