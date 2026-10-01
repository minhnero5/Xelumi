Shader "Custom/CutterMask"
{
    SubShader
    {
        Tags { "Queue" = "Geometry" }
        ColorMask 0 // Don't render anything
        ZWrite On
        Stencil
        {
            Ref 1
            Comp Always
            Pass Replace
        }
        Pass {} // Empty pass to apply stencil settings
    }
}
