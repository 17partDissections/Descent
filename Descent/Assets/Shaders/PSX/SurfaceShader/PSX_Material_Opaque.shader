Shader "PSX/PSX_Material_Opaque"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _JitterIntensity ("Vertex Jitter", Range(1,500)) = 150
        _AffineStrength ("Affine Strength", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass
        {
            ZWrite On
            ZTest LEqual
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _JitterIntensity;
            float _AffineStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uvPersp : TEXCOORD0;
                float2 uvAffine : TEXCOORD1;
                float  w : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;

                float4 viewPos = mul(UNITY_MATRIX_MV, v.vertex);
                viewPos.xyz = floor(viewPos.xyz * _JitterIntensity) / _JitterIntensity;
                float4 clipPos = mul(UNITY_MATRIX_P, viewPos);

                o.pos = clipPos;

                float2 uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvPersp = uv;
                o.uvAffine = uv * clipPos.w;
                o.w = clipPos.w;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 affineUV = i.uvAffine / i.w;
                float2 finalUV = lerp(i.uvPersp, affineUV, _AffineStrength);
                return tex2D(_MainTex, finalUV) * _Color;
            }

            ENDCG
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }

            ZWrite On
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vertShadow
            #pragma fragment fragShadow
            #include "UnityCG.cginc"

            float _JitterIntensity;

            struct v2f { V2F_SHADOW_CASTER; };

            v2f vertShadow(appdata_base v)
            {
                v2f o;

                float4 viewPos = mul(UNITY_MATRIX_MV, v.vertex);
                viewPos.xyz = floor(viewPos.xyz * _JitterIntensity) / _JitterIntensity;
                o.pos = mul(UNITY_MATRIX_P, viewPos);

                return o;
            }

            float4 fragShadow(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }

            ENDCG
        }
    }

    Fallback Off
}