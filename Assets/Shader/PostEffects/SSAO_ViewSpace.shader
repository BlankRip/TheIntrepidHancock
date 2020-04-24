Shader "Custom/PostEffects/SSAO_ViewSpace"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
//#include "UnityShaderVariables.cginc"
// #include "UnityShaderUtilities.cginc"
// #include "UnityInstancing.cginc"

//#include "Support.cginc"
			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);

			uniform float4x4 _InverseProjectionMatrix;
			uniform float4x4 _ProjectionMatrix;

		//	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	//	sampler2D _MainTex;
//		sampler2D _CameraDepthTexture;
//		sampler2D _CameraDepthNormalsTexture;
		float _Blend;
		float _SampleShift;
		float _ScanDistance;
		float _MaxRange;

		float random(float2 st) {
			return frac(sin(dot(st.xy , float2(121.9891, 785.237)))*4132517.5453123);
		}
/*
		// returns a wo
		float4 RandomRay(float2 screenPos, float factor)
		{
			screenPos *= 2;
			float3 screenNormal = float3(random(screenPos + float2(0.71, -0.1)), random(screenPos + float2(-0.25, -0.5)), random(screenPos + float2(0.17, 0.7)));
			screenNormal = normalize((screenNormal * 2) - 1) * factor;
		//	screenNormal = mul(unity_MatrixVP, screenNormal);
	//		screenNormal = ;
			//return mul(unity_MatrixVP, screenNormal) * factor;
			float4 randomViewVector = mul(unity_WorldToCamera, screenNormal);
		//	randomViewVector.xyz *= factor;
			return randomViewVector;
		}

	*/
		float BiasCtrl(float value)
		{
			return saturate(1 - pow(value * _MaxRange, 2));
		}

/*
		float AmbiantFactor(float2 screenPos) 
		{
			float SampleCount = 64;

			float screenSimpleDepth = tex2D(_CameraDepthTexture, screenPos).r;
			float pointDepth = LinearEyeDepth(screenSimpleDepth);
			float factor = 0;
			half3 randomDir;
		
			for (float i = 1; i < SampleCount; i++)
			{
				float4 screenRay = RandomRay(screenPos + float2(sin(i), cos(i + 8.7)), (1 - Linear01Depth (screenSimpleDepth)) * 0.5 *  _ScanDistance * (1 - (i/SampleCount))) ;
				float2 screenDepthSample = mul(unity_MatrixVP, screenRay);
		//		float shiftSimpleDepth = tex2D(_CameraDepthTexture, screenPos + (screenRay.xy * _ScanDistance * (screenSimpleDepth) * _SampleShift)).r;
				float shiftSimpleDepth = tex2D(_CameraDepthTexture, screenPos + (screenDepthSample.xy * _SampleShift)).r;
			
				float shiftPointDepth = LinearEyeDepth(shiftSimpleDepth);
		//		float diffValue = pointDepth - shiftPointDepth;
		//		factor += max(diffValue, 0) * (i/SampleCount) * BiasCtrl(diffValue);
				factor += step(pointDepth - screenRay.z/screenRay.w, shiftPointDepth) * (i/SampleCount);
			}

			return pow(factor/SampleCount, _Blend);

		}
		*/

			float4 AmbiantFactor(float2 screenPos) 
		{
			/*
			float SampleCount = 64;

			float screenSimpleDepth = (tex2D(_CameraDepthTexture, screenPos)).r;
		//	float pointDepth = LinearEyeDepth(screenSimpleDepth);
			float pointDepth = Linear01Depth(screenSimpleDepth);
			float2 pt = mul(unity_StereoMatrixV, float3(0,0,0,0));
		//	return pow(factor/SampleCount, _Blend);
		//	return float4(screenPos.x, screenPos.y, 0, 0);
		*/

//float2 uv = i.texcoordStereo;
	float2 uv = screenPos;
    float depth = _CameraDepthTexture.SampleLevel(sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(uv),0).r;
    float4 viewSpacePos = mul(_InverseProjectionMatrix, float4((uv * 2) - 1, depth, 1));
 //   viewSpacePos.xyz /= viewSpacePos.w;
 //   return -viewSpacePos.z * 0.01;
		float2 newUV = (mul(_ProjectionMatrix, viewSpacePos).xy + 1 ) * 0.5;
	//	return _CameraDepthTexture.SampleLevel(sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(newUV),0).r * 10;
	//	return pointDepth;
	//	return float4(newUV , 0,0);
return _CameraDepthTexture.SampleLevel(sampler_CameraDepthTexture, newUV,0).r * 10;
		}


	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

	//	return (1 - saturate(AmbiantFactor(i.texcoord)));
	//	return float4(RandomRay(i.texcoord, 1), 1);
	//	return saturate(AmbiantFactor(i.texcoord));
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