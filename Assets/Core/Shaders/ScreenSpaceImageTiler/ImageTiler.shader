Shader "Nytro/ImageTiler"
{
    Properties
    {
        _BaseMap("Base Map", 2D) = "white"
        _ScaleScroll("Scale and Scrolling", Vector) = (1, 1, 0, 0)
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent" 
            "RenderPipeline" = "UniversalRenderPipeline" 
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float4 vertexColor  : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float4 screenPos    : TEXCOORD0;
                float4 vertexColor       : COLOR;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _ScaleScroll;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                OUT.vertexColor = IN.vertexColor;

                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 screenPos = (IN.screenPos.xy / IN.screenPos.w);

                // unpack variables
                float2 scale = _ScaleScroll.xy;
                float2 scroll = _ScaleScroll.zw;
                float width = _ScreenParams.x;
                float height = _ScreenParams.y;
                float4 vertexColor = IN.vertexColor;

                float2 pixelPos = float2(screenPos.x * (width / height), screenPos.y);

                // scale
                pixelPos.x *= scale.x;
                pixelPos.y *= scale.y;

                // time
                pixelPos.x += _Time.x * scroll.x;
                pixelPos.y += _Time.x * scroll.y;

                float4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, pixelPos);

                return color * vertexColor;
            }
            ENDHLSL
        }
    }
}
