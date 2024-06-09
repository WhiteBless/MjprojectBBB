Shader "Unlit/UI_Glow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlowColor("Glow Color", Color) = (1,1,1,1)
        _GlowIntensity("Glow Intensity", Range(0, 5)) = 1
    }
    SubShader
    {
        Tags {"Queue" = "Overlay" "RenderType" = "Transparent"}
        LOD 100

        Pass
        {
          CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _GlowColor;
            float _GlowIntensity;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.texcoord;
                fixed4 texColor = tex2D(_MainTex, uv);
                float glow = abs(sin(_Time.y)) * _GlowIntensity;
                texColor.rgb += _GlowColor.rgb * glow;
                return texColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
