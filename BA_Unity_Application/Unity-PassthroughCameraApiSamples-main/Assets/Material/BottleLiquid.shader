Shader "Model/BottleLiquid"
{
    Properties
    {
        _WaveCenter("Wave Center", Vector) = (0.0, 0.0, 0.0, 0.0)
        _WaveParams("Wave Params", Vector) = (0.0, 0.0, 0.0, 0.0)
        _LiquidColorForward("Liquid Color Forward", Color) = (0.5, 0.5, 0.5, 1)
        _LiquidColorBack("Liquid Color Back", Color) = (0.8, 0.8, 0.8, 1)
    }
    SubShader
    {
        Tags {"RenderType" = "Opaque" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline" "ShaderModel"="4.5"}
        LOD 100

        Blend One Zero
        ZWrite On
        ZTest LEqual

        // Two-sided rendering
        Cull Off

        Pass
        {
            Name "BottleLiquid"

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            #pragma vertex vert
            #pragma fragment frag

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _WaveCenter;
            float4 _WaveParams;
            float4 _LiquidColorForward;
            float4 _LiquidColorBack;

            #define WaveSize (_WaveParams.x)
            #define WaveCycleCoef (_WaveParams.y)
            #define WaveOffsetCycleCoef (_WaveParams.z)

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 vertex   : SV_POSITION;
                float3 posWS    : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDir  : TEXCOORD2;
                float fogCoord  : TEXCOORD3;

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);

                output.vertex = vertexInput.positionCS;
                output.posWS = vertexInput.positionWS;
                output.normalWS = NormalizeNormalPerPixel(normalInput.normalWS);
                output.viewDir = GetWorldSpaceViewDir(vertexInput.positionWS);
                output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
                return output;
            }

            half4 frag(Varyings input) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(input);

                // Calculate the wave height from the set parameters and local XZ coordinates
                float waveBaseY = _WaveCenter.y;
                float2 localXZ = _WaveCenter.xz - input.posWS.xz;
                float2 waveInput = localXZ * WaveCycleCoef;
                float waveInputOffset = _Time.y * WaveOffsetCycleCoef;
                waveInput += waveInputOffset;
                float clipPosY = waveBaseY + (sin(waveInput.x) + sin(waveInput.y)) * WaveSize;

                // Discard pixels above the calculated Y position
                clip(clipPosY - input.posWS.y);

                // For the remaining lower pixels, check the normal direction and color the front/back faces differently.
                // The part displayed on the back face will look like the water surface.
                half NdotV = dot(input.normalWS, input.viewDir);
                half4 color = lerp(_LiquidColorBack, _LiquidColorForward, step(0.0h, NdotV));

                color.rgb = MixFog(color.rgb, input.fogCoord);
                color.a = 1.0h;

                return color;
            }

            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
