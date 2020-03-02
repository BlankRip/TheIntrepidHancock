Shader "Custom/Trail/StrikeTrail"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Pattern("Pattern", 2D) = "white" {}
		_PatternB("Pattern B", 2D) = "white" {}
		_CloudNoise("Cloud Noise", 2D) = "white" {}
		_RageSpeed("Rush Speed", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Opaque" }
		ZTest Off
		Cull OFF
		Blend One One
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float4 color : COLOR;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _Pattern, _PatternB, _CloudNoise;
            float4 _MainTex_ST;
			float _RageSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
				col *= i.color.a;
				float3 distTexture = tex2D(_CloudNoise, (i.uv * 0.2) + fixed2(-_Time.x * _RageSpeed, 0));
				float value = tex2D(_Pattern, fixed2(i.uv.x + 0.5, _Time.x + i.uv.y + distTexture.x * 0.3)).r * 2;
				float valueB = tex2D(_PatternB, i.uv + fixed2(i.uv.x - 0.5, _Time.x + i.uv.y - distTexture.x * 0.3)).r * 2;
		//		float pointCol = lerp(tex2D(_Pattern, i.uv + fixed2(0, 0.2)).r, tex2D(_Pattern, i.uv + fixed2(0, -0.2)).r, tex2D(_CloudNoise, i.uv + fixed2(-_Time.y, 0.2)).r);
               // return col * i.color * lerp(1, 0, value);
				return col * i.color * lerp(value, valueB, distTexture.z);
            }
            ENDCG
        }
    }
}
