Shader "MyShader/WavingFlag"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NoiseTex("Noise tex", 2D) = "gray" {} 
        _Amount ("Movement Amount", Range(-1,1)) = 0.5
        _Speed ("Movement Speed", Range(0,2)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert alphatest:_Cutoff vertex:vert addshadow


        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _NoiseTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float _Amount;
        fixed _Speed;

        void vert (inout appdata_full v) {
             float3 disp = (tex2Dlod(_NoiseTex, float4(v.texcoord.xy + (fixed2(_Time.x, _Time.x) * _Speed),0,0)) - 0.5) * 2 ;   
             v.vertex.xyz += v.normal * disp * _Amount * v.color.r; 
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Alpha = c.a;
            clip(c.a - 0.5);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
