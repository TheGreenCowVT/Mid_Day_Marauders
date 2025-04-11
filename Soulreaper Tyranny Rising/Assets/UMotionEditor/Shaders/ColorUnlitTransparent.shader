Shader "UMotion Editor/Color Unlit Transparent"		
{
    Properties
    {
        // Color property for material inspector, default to white
        Color ("Main Color", Color) = (1,1,1,1)
    }
    SubShader
    {
    	Tags { "RenderType"="Transparent" "IgnoreProjector"="True" }
		LOD 100
    	ZWrite Off
    	Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            // vertex shader
            // this time instead of using "appdata" struct, just spell inputs manually,
            // and instead of returning v2f struct, also just return a single output
            // float4 clip position
            float4 vert (float4 vertex : POSITION) : SVPOSITION
            {
                return UnityObjectToClipPos(vertex);
            }
            
            // color from the material
            fixed4 Color;

            // pixel shader, no inputs needed
            fixed4 frag () : SVTarget
            {
                return Color; // just return it
            }
            ENDCG
        }
    }
}