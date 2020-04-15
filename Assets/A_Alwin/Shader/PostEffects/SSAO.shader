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
		float _SampleShift;
		float _ScanDistance;
		float _MaxRange;

		float random(float2 st) {
			return frac(sin(dot(st.xy , float2(121.9891, 785.237)))*4132517.5453123);
		}

		float3 RandomRay(float2 screenPos, float factor)
		{
			screenPos *= 2;
			float3 screenNormal = float3(random(screenPos + float2(0.71, -0.1)), random(screenPos + float2(-0.25, -0.5)), random(screenPos + float2(0.17, 0.7)));
			screenNormal = normalize((screenNormal * 2) - 1);
			screenNormal = mul(unity_MatrixVP, screenNormal);
			return screenNormal * factor;
		}

	
		float BiasCtrl(float value)
		{
			return saturate(1 - pow(value * _MaxRange, 5));
		}


		float AmbiantFactor(float2 screenPos) 
		{
			float SampleCount = 64;

			float screenSimpleDepth = tex2D(_CameraDepthTexture, screenPos).r;
			float pointDepth = LinearEyeDepth(screenSimpleDepth);
			float factor = 0;
			half3 randomDir;
		
			for (float i = 1; i < SampleCount; i++)
			{
				float3 screenRay = RandomRay(screenPos + float2(sin(i), cos(i + 8.7)), (1 - (i/SampleCount))) ;
				float shiftSimpleDepth = tex2D(_CameraDepthTexture, screenPos + (screenRay.xy * _ScanDistance * (screenSimpleDepth) * _SampleShift)).r;
				float shiftPointDepth = LinearEyeDepth(shiftSimpleDepth);
				float diffValue = pointDepth - shiftPointDepth;
				factor += max(diffValue, 0) * (i/SampleCount) * BiasCtrl(diffValue);
				
			}
			return 1 - pow(factor/SampleCount, _Blend) * 5;

		}
		


	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

	//	return (1 - saturate(AmbiantFactor(i.texcoord)));
	//	return float4(RandomRay(i.texcoord, 1), 1);
	return color * saturate(AmbiantFactor(i.texcoord));
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