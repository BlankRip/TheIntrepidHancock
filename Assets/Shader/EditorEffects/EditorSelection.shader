Shader "Unlit/EditorHeilight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend OneMinusDstColor One
        ZWrite Off
        ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _FlashColor;
            float _Flicker;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = lerp(_FlashColor, 0, sin(_Time.z * 10) * _Flicker);
                return col;
               // return col (sin(_Time.z * 10)) * 0.1;
            }
            ENDCG
        }
    }
}
