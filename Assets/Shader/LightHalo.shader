Shader "MyShader/Billboard/Halo_2"
{
    Properties
    {
        _Color ("Color", color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Scale("Scale", float) = 1
    }
    SubShader
    {
		Tags{ "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
 
		Lighting Off
			ZWrite On
			Blend OneMinusDstColor One // Soft Additive
			Cull Back
            ZTest Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag       
            #include "UnityCG.cginc"
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 screenPosition : TEXCOORD1;
                float4 centerPointValue : TEXCOORD2;
            };

            sampler2D _CameraDepthTexture;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed _Scale;
            
            v2f vert (appdata_base v)
            {
                v2f o;

                fixed4 objectScreenPoint = mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
                o.color = objectScreenPoint;
                o.pos = mul(UNITY_MATRIX_P,
                objectScreenPoint + float4(v.vertex.x, v.vertex.y, 0, 0) * _Scale) ;


                o.centerPointValue = mul(UNITY_MATRIX_P, objectScreenPoint) ;

            //    o.pos = UnityObjectToClipPos(v.vertex);

                o.screenPosition = ComputeScreenPos(o.centerPointValue);

                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 screenUV = i.screenPosition.xy/ i.screenPosition.w;
                float screenSimpleDepth = tex2D(_CameraDepthTexture, screenUV).r;
			    float pointDepth = LinearEyeDepth(screenSimpleDepth);

            //    fixed4 col = saturate(tex2D(_MainTex, i.uv) * _Color * max( 1- i.color.z * 0.1, 0));
               fixed4 col = saturate(tex2D(_MainTex, i.uv) * _Color) * step(-i.color.z/i.color.w, pointDepth);
       //         return (-i.color.z * 0.01);
                return col;
            }
            ENDCG
        }
    }
}