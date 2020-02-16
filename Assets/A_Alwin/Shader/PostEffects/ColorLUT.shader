Shader "Custom/PostEffects/ColorLUT"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

		uniform TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		uniform TEXTURE2D_SAMPLER2D(_LUTTex, sampler_LUTTex);
		float4 _LUTTex_TexelSize;
		float _Blend;

	//	#define COLORS 32.00000000000000

	float4 Frag(VaryingsDefault i) : SV_Target
	{
				float COLORS = 32.0;
		  		float maxColor = COLORS - 1.0;
                float4 col = saturate(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord));
                float halfColX = 0.5 / _LUTTex_TexelSize.z;
                float halfColY = 0.5 /_LUTTex_TexelSize.w;
                float threshold = maxColor / COLORS;
 
                float xOffset = halfColX + (col.r / COLORS) * threshold;
                float yOffset = halfColY + (col.g * threshold);

				// find both the cells so we could extrapolate
				float blueValue = col.b * maxColor;

                float cell_floor = floor(blueValue);
				float cell_celing = ceil(blueValue);

				float extrapolat = frac(blueValue);
 
                float2 lutPos_floor = float2((cell_floor / COLORS) + xOffset, yOffset);
				float2 lutPos_celing = float2((cell_celing / COLORS) + xOffset, yOffset);

				float4 col_floor = SAMPLE_TEXTURE2D(_LUTTex, sampler_LUTTex, lutPos_floor);
				float4 col_celing = SAMPLE_TEXTURE2D(_LUTTex, sampler_LUTTex, lutPos_celing);

				return lerp(col_floor, col_celing, extrapolat);
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