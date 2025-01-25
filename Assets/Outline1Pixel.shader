Shader "Sprites/Outline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Main texture Tint", Color) = (1,1,1,1)
        _SolidOutline ("Outline Color", Color) = (1,1,1,1)  // Default white outline
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ OUTLINE_ON
            #pragma exclude_renderers d3d11_9x

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _SolidOutline;
            uniform float4 _MainTex_TexelSize;  // Texel size for 1 pixel outline

            // Vertex function to pass the texture coordinate and vertex position to the fragment shader
            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            // Function to sample the sprite texture
            fixed4 SampleSpriteTexture(float2 uv)
            {
                return tex2D(_MainTex, uv);
            }

            // Check if a transparent pixel is adjacent to a non-transparent pixel
            bool IsTransparentAndAdjacentToOpaque(float2 uv)
            {
                fixed4 centerPixel = SampleSpriteTexture(uv);

                // If this pixel is transparent
                if (centerPixel.a < 0.1)
                {
                    // Check the neighboring pixels for non-transparent pixels
                    float2 thicknessX = float2(_MainTex_TexelSize.x, 0);
                    float2 thicknessY = float2(0, _MainTex_TexelSize.y);

                    return SampleSpriteTexture(uv + thicknessX).a > 0.1 ||  // Right
                           SampleSpriteTexture(uv - thicknessX).a > 0.1 ||  // Left
                           SampleSpriteTexture(uv + thicknessY).a > 0.1 ||  // Up
                           SampleSpriteTexture(uv - thicknessY).a > 0.1;    // Down
                }

                return false;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 spriteColor = SampleSpriteTexture(IN.texcoord) * IN.color;
                spriteColor.rgb *= spriteColor.a;

                // Outline processing, only if the outline is enabled
                #ifdef OUTLINE_ON
                // If the current pixel is transparent and adjacent to non-transparent pixels
                if (IsTransparentAndAdjacentToOpaque(IN.texcoord))
                {
                    // Apply the outline color (white in this case)
                    return _SolidOutline;
                }
                #endif

                return spriteColor;  // Return the original sprite color if not a border pixel
            }
        ENDCG
        }
    }
}
