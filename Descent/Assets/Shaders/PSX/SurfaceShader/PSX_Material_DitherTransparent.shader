Shader "PSX/PSX_DitherTransparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _JitterIntensity ("Vertex Jitter", Range(10,500)) = 150
        _Transparency ("Transparency", Range(0,1)) = 0.5
        _DitherScale ("Dither Scale", Float) = 1
        _AffineStrength ("Affine Strength", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }

        Pass
        {
            ZWrite On
            ZTest LEqual
            Cull Back
            Blend Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _JitterIntensity;
            float _Transparency;
            float _DitherScale;
            float _AffineStrength;

            static const float4x4 ditherMatrix = {
                0,8,2,10,
                12,4,14,6,
                3,11,1,9,
                15,7,13,5
            };

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
                float4 screenPos : TEXCOORD3;
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

                o.screenPos = ComputeScreenPos(clipPos);
                return o;
            }

            float GetDither(float2 sp)
            {
                sp *= _ScreenParams.xy * _DitherScale;
                uint x = (uint)sp.x % 4;
                uint y = (uint)sp.y % 4;
                return ditherMatrix[y][x] / 16.0;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 affineUV = i.uvAffine / i.w;
                float2 finalUV = lerp(i.uvPersp, affineUV, _AffineStrength);
                
                fixed4 col = tex2D(_MainTex, finalUV) * _Color;

                float2 sp = i.screenPos.xy / i.screenPos.w;
                clip(col.a * _Transparency - GetDither(sp));

                return col;
            }

            ENDCG
        }
    }

    Fallback Off
}