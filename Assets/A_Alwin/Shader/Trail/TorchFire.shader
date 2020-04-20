Shader "Custom/Trail/TorchFire"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Pattern("Pattern", 2D) = "white" {}
		_PatternB("Pattern B", 2D) = "white" {}
		_FameGradient("Flame Gradient", 2D) = "black" {}
		_NoiseStretch("Rush Speed", Vector) = (0,0,0,0)
		_RageSpeed("Rush Speed", Range(0, 10)) = 1
		_FlameBias("Flame Bias", Range(0, 5)) = 1
		_DistortionPower("Flame Bias", Range(0, 1)) = 1

    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Opaque" }
		//ZTest Off
		Cull OFF
		Blend OneMinusDstColor One
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
//            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float4 color : COLOR;
   ///             UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _Pattern, _PatternB, _FameGradient;
            float4 _MainTex_ST, _NoiseStretch;
			float _RageSpeed, _FlameBias, _DistortionPower;

            v2f vert (appdata v)
            {
                v2f o;

			//	fixed3 shift = tex2dLod(_CloudNoise, );


				float effectFactor = tex2Dlod(_MainTex, float4(v.uv.xy, 0, 0)).x;

				float3 disp = (tex2Dlod(_Pattern, float4(fixed2(v.uv.x * _NoiseStretch.z, v.uv.y * _NoiseStretch.w) + fixed2(-_Time.y  * _RageSpeed, 0), 0, 0)) * 2) - 1;

				v.vertex.xy *= effectFactor;

                o.vertex = UnityObjectToClipPos(v.vertex + disp * _DistortionPower * (1 - effectFactor));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
           //     UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed col = tex2D(_MainTex, i.uv).r;
                // apply fog
           //     UNITY_APPLY_FOG(i.fogCoord, col);
				col *= i.color.a;
				fixed distTexture = tex2D(_PatternB, fixed2(i.uv.x * _NoiseStretch.x, i.uv.y * _NoiseStretch.y) + fixed2(-_Time.y * _RageSpeed, 0)).r;
			//	float value = tex2D(_Pattern, fixed2(i.uv.x + 0.5, _Time.x + i.uv.y + distTexture.x * 0.3)).r * 2;
			//	float valueB = tex2D(_PatternB, i.uv + fixed2(i.uv.x - 0.5, _Time.x + i.uv.y - distTexture.x * 0.3)).r * 2;
		//		float pointCol = lerp(tex2D(_Pattern, i.uv + fixed2(0, 0.2)).r, tex2D(_Pattern, i.uv + fixed2(0, -0.2)).r, tex2D(_CloudNoise, i.uv + fixed2(-_Time.y, 0.2)).r);
               // return col * i.color * lerp(1, 0, value);
			//	return col * i.color * lerp(value, valueB, distTexture.z);
				return  tex2D(_FameGradient, fixed2(1 - saturate(col - pow(distTexture, _FlameBias)), 0.5));
            }
            ENDCG
        }
    }
}
