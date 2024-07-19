Shader "Unlit/SpriteOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0, 20)) = 1
        
        _smoothStep ("Smoothstep", Vector) = (0.95, 0.98, 0.5, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        
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
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;            

            fixed4 _OutlineColor;
            float _OutlineWidth;

            float4 _smoothStep;
            
            static float D = 0.7;
            static float2 _dirs[8] = {float2(1, 0), float2(-1, 0), float2(0, 1), float2(0, -1),
                                      float2(D, D), float2(-D, D), float2(D, -D), float2(-D, -D)};

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            float GetMaxAlpha(float2 uv)
            {
                float result = 0;

                for (uint i = 0; i < 8; i++)
                {
                    float2 sUV = uv + _dirs[i] * _MainTex_TexelSize.xy * _OutlineWidth ;
                    result += tex2D(_MainTex, sUV).a;
                }
                
                return result / 8;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= i.color;

                float a = GetMaxAlpha(i.uv);
                float outlineMask = 1 - smoothstep(_smoothStep.x, _smoothStep.y, 1 - a) - col.a; // smoothstep(0.1, 0.2, 1 - a); // lerp(_OutlineColor, col.rgb, col.a);

                // col.rgb = smoothstep(0.5, 1, a); // smoothstep(0.1, 0.2, 1 - a); // lerp(_OutlineColor, col.rgb, col.a);
                // col.rgb = col.rgb;
                
                col.rgb = lerp(col.rgb, _OutlineColor, outlineMask); 
                col.a = smoothstep(_smoothStep.z, _smoothStep.w, col.a) + outlineMask * _OutlineColor.a;
                
                return col;
            }
            ENDCG
        }
    }
}
