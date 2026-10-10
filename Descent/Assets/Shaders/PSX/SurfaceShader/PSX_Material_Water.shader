Shader "PSX/PSX_Material_Water"
{
    Properties
    {
        _WaterColor ("Water Base Color", Color) = (0.2,0.4,0.6,0.8)
        _MainTex ("Primary Texture", 2D) = "white" {}
        _SecondTex ("Distortion Texture", 2D) = "gray" {}
        _TextureScrollSpeedX ("Scroll X", Float) = 0.05
        _TextureScrollSpeedY ("Scroll Y", Float) = 0.05
        _SecondTextureScrollSpeedX ("Distort Scroll X", Float) = 0.03
        _SecondTextureScrollSpeedY ("Distort Scroll Y", Float) = 0.03
        _DistortionStrength ("Distortion", Range(0,0.5)) = 0.05
        _JitterIntensity ("Vertex Jitter", Range(10,500)) = 150
        _ColorQuantizationLevels ("Color Steps", Range(1,64)) = 16
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
            sampler2D _SecondTex;

            float4 _WaterColor;
            float _TextureScrollSpeedX;
            float _TextureScrollSpeedY;
            float _SecondTextureScrollSpeedX;
            float _SecondTextureScrollSpeedY;
            float _DistortionStrength;
            float _JitterIntensity;
            float _ColorQuantizationLevels;
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
                float2 baseUV = lerp(i.uvPersp, affineUV, _AffineStrength);

                float2 uvDistort = baseUV + float2(_Time.y * _SecondTextureScrollSpeedX,
                                                    _Time.y * _SecondTextureScrollSpeedY);

                float2 distortion = (tex2D(_SecondTex, uvDistort).rg * 2 - 1) * _DistortionStrength;

                float2 uvMain = baseUV + distortion +
                                float2(_Time.y * _TextureScrollSpeedX,
                                       _Time.y * _TextureScrollSpeedY);

                fixed4 texCol = tex2D(_MainTex, uvMain);

                fixed4 finalCol = _WaterColor * texCol;
                finalCol.rgb = floor(finalCol.rgb * _ColorQuantizationLevels) / _ColorQuantizationLevels;

                clip(finalCol.a - 0.01);

                return finalCol;
            }

            ENDCG
        }
    }

    Fallback Off
}