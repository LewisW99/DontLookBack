Shader "Hidden/RetroBlit_VHSGlitch"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _BitDepth ("Bit Depth", Float) = 4
        _DitherStrength ("Dither Strength", Float) = 1
        _CRTWarp ("CRT Warp Amount", Float) = 0.0
        _GrainStrength ("Grain Strength", Float) = 0.05
        _EnableShear ("Enable Shearing", Float) = 0
        _ShearStrength ("Shear Strength", Float) = 0.02
        _EnableJump ("Enable Scanline Jump", Float) = 0
        _EnableRGBSplit ("Enable RGB Split", Float) = 0
        _RGBSplitAmount ("RGB Split Amount", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "LightMode" = "SRPDefaultUnlit" }

        Pass
        {
            ZTest Always Cull Off ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;

            CBUFFER_START(UnityPerMaterial)
            float _BitDepth;
            float _DitherStrength;
            float _CRTWarp;
            float _GrainStrength;
            float _EnableShear;
            float _ShearStrength;
            float _EnableJump;
            float _EnableRGBSplit;
            float _RGBSplitAmount;
            CBUFFER_END

            float4 _MainTex_TexelSize;


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 screenUV : TEXCOORD1;
            };

            float bayerThreshold4x4(int2 pixel)
            {
                int x = pixel.x % 4;
                int y = pixel.y % 4;
                const float thresholdMatrix[4][4] = {
                    { 0.0/16.0,  8.0/16.0,  2.0/16.0, 10.0/16.0 },
                    {12.0/16.0,  4.0/16.0, 14.0/16.0,  6.0/16.0 },
                    { 3.0/16.0, 11.0/16.0,  1.0/16.0,  9.0/16.0 },
                    {15.0/16.0,  7.0/16.0, 13.0/16.0,  5.0/16.0 }
                };
                return thresholdMatrix[y][x];
            }

            float2 ApplyCRTWarp(float2 uv, float warpAmount)
            {
                float2 center = float2(0.5, 0.5);
                float2 offset = uv - center;
                float dist = dot(offset, offset);
                return uv + offset * dist * warpAmount;
            }

            float grainNoise(int2 pixelCoord, float timeSeed)
            {
                float seed = dot(pixelCoord, int2(12, 78)) + timeSeed;
                return frac(sin(seed) * 43758.5453);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenUV = (o.pos.xy / o.pos.w) * 0.5 + 0.5;
                o.screenUV *= _ScreenParams.xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                int2 pixelCoord = int2(i.screenUV);

                // Optional scanline jump
                if (_EnableJump > 0.5 && frac(_Time.y * 0.5) < 0.05)
                {
                    uv.y += 0.02;
                }

                // Optional horizontal shear/desync bars
                if (_EnableShear > 0.5 && frac(i.screenUV.y * 0.05 + _Time.y * 0.5) < 0.1)
                {
                    uv.x += sin(_Time.y * 60.0) * _ShearStrength;
                }

                // Optional CRT warp
                uv = ApplyCRTWarp(uv, _CRTWarp);

                fixed4 col;

                if (_EnableRGBSplit > 0.5)
                {
                    // Offset for split (based on screen res)
                    float2 splitOffset = float2(_RGBSplitAmount / _ScreenParams.x, 0);

                    float r = tex2D(_MainTex, uv + splitOffset).r;
                    float g = tex2D(_MainTex, uv).g;
                    float b = tex2D(_MainTex, uv - splitOffset).b;
                    col = float4(r, g, b, 1.0);
                }
                else
                {
                    col = tex2D(_MainTex, uv);
                }

                // Bayer Dithering
                float threshold = bayerThreshold4x4(pixelCoord);
                float levels = pow(2.0, _BitDepth);
                col.rgb = floor(col.rgb * levels + threshold * _DitherStrength) / (levels - 1);

                // Film grain
                float grain = grainNoise(pixelCoord, _Time.y * 60.0);
                col.rgb += (grain - 0.5) * _GrainStrength;

                return saturate(col);
            }
            ENDHLSL
        }
    }
}
