Shader "Custom/BlackMaskShader"
{
    Properties
    {
        _Color("Color",COLOR)=(1,1,1,0)
        _Mask("Mask", Range(0,2)) = 0
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Mask;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float distance = abs(i.uv.x + i.uv.y - 1) / sqrt(2.0);

                // Calculate mask value
                float maskValue = -sqrt(2.0) / 2 + _Mask + distance;

                if (maskValue <= 0)
                {
                    maskValue = 0;
                }

                if (maskValue >= 1)
                {
                    maskValue = 1;
                }

                fixed4 finalColor = _Color;

                finalColor.a=maskValue;

                return finalColor;
            }
            ENDCG
        }
    }
}