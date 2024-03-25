Shader "Custom/BasicTexture"
{
    Properties
    {
        _Color("AlphaSetColor",COLOR)=(1,1,1,0)
        _Texture("Texture",2D)=""{}
    }
    SubShader
    {
        Pass
        {
            Cull Back

            SetTexture[_Texture]
            {
                Combine Texture
            }
        }
        Pass
        {
            Cull Front

            Blend SrcAlpha OneMinusDstColor

            SetTexture[_Texture]
            {
                ConstantColor[_Color]
                Combine Texture*constant Double
            }
        }
    }
    FallBack "Diffuse"
}
