Shader "Custom/MapOverlay" {
    Properties {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Offset ("Offset", Vector) = (0, 0, 0, 0)
        _WorldSize ("World Size", Vector) = (100, 100, 0, 0)
        _PixelScale ("Pixel Scale", Float) = 1.0
        _CircleRadius ("Radius", Float) = 0.0
        _CircleThickness ("Thickness", Float) = 0.0
    }
    SubShader {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            #define APPLY_COLOR(c) { \
                if (c.a > 0.0) {     \
                    finalColor += c; \
                    totalColors++;   \
                }                    \
            }
            
            float2 _Offset;
            float2 _WorldSize;
            float _PixelScale;
            float _CircleRadius;
            float _CircleThickness;

            v2f vert (appdata vertex) {
                v2f output;
                output.uv = vertex.uv;
                output.pos = UnityObjectToClipPos(vertex.vertex);
                return output;
            }

            half4 frag (v2f input) : SV_Target {
                float2 worldLocalPosition = (input.uv - 0.5) * _WorldSize; // The - 0.5 makes the coords relative to center of object
                float2 worldGlobalPosition = worldLocalPosition - _Offset;
                float2 position = floor(worldGlobalPosition);

                half4 finalColor = half4(0.0, 0.0, 0.0, 0.0);
                int totalColors = 0;

                // Circle
                if (_CircleRadius > 0) {
                    float circleThickness = min(_CircleRadius, _CircleThickness);
                    half4 circleColor = abs(_CircleRadius - distance(float2(0, 0), position)) <= circleThickness / 2.0 ? half4(0.96, 0.62, 0.11, 0.5) : half4(0.0, 0.0, 0.0, 0.0);
                    APPLY_COLOR(circleColor);
                }
                
                return totalColors > 0 ? finalColor / totalColors : half4(0.0, 0.0, 0.0, 0.0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
