Shader "Custom/ZTestShader"
{
    Properties
    {
        _SufaceColor("SurfaceColor",COLOR)=(1,1,1,0)
    }
    SubShader
    {
        Pass
        {
            ZTest LEqual
            COLOR[_SufaceColor]
        }
        Pass
        {
            ZTest Greater
            ZWrite Off
            COLOR(1,1,1,0)
        }
    }
    FallBack "Diffuse"
}
