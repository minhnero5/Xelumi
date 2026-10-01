Shader "Custom/ClippedObject"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}  // Texture field
        _Color ("Color Tint", Color) = (1,1,1,1)    // Color tint for main texture
        _EmissionTex ("Emission Texture", 2D) = "black" {} // Emission texture
        _EmissionColor ("Emission Color", Color) = (1,1,1,1) // Glow color
        _EmissionStrength ("Emission Strength", Float) = 1.0 // Glow intensity
    }
    SubShader
    {
        Tags { "Queue" = "Geometry" "RenderType" = "Opaque" }
        
        Stencil
        {
            Ref 1
            Comp NotEqual // Render only outside the cutter
        }

        CGPROGRAM
        #pragma surface surf Standard addshadow fullforwardshadows // Enable shadows

        sampler2D _MainTex;
        sampler2D _EmissionTex;
        fixed4 _Color;
        fixed4 _EmissionColor;
        float _EmissionStrength;

        struct Input
        {
            float2 uv_MainTex; // UV mapping for main texture
            float2 uv_EmissionTex; // UV mapping for emission texture
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Sample the main texture
            fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = texColor.rgb * _Color.rgb;  // Apply texture with color tint

            // Sample the emission texture
            fixed4 emissionTex = tex2D(_EmissionTex, IN.uv_EmissionTex);
            o.Emission = emissionTex.rgb * _EmissionColor.rgb * _EmissionStrength;

            o.Metallic = 0.0;
            o.Smoothness = 0.5; // Add smoothness for better shading
        }
        ENDCG
    }
    FallBack "Standard"
}
