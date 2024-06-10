Shader "Custom/MatrixCodeShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Speed ("Scroll Speed", Float) = 0.5 
        _Fade ("Fade Amount", Float) = 0.5
        _Distortion ("Distortion Amount", Float) = 0.1 
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

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
            float _Speed;
            float _Fade;
            float _Distortion;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv + float2(0, _Distortion); 
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                float time = _Time.y; 

                
                float2 uv = i.uv;
                uv.y += time * _Speed;

                
                fixed4 col = tex2D(_MainTex, uv);

               
                float greenTint = lerp(0.8, 1.0, col.g);
                col.rgb = col.g * float3(0.0, greenTint, 0.0);
                
                float fade = (1.0 - smoothstep(0.0, 1.0, frac(uv.y))) * _Fade;

                return col * fade;
            }
            ENDCG
        }
    }
}
