Shader "UMotion Editor/Unlit Dashed Line"
{ 
    Properties 
    {
		Color("Line Color (RGB) Trans (A)", color) = (0, 0, 0, 1)
		Thickness("Line Thikness", Range(0, 4)) = 0.9
		DashFrequency("Dash Frequency", Range(0, 150)) = 100
    }

    SubShader 
    {
		Tags {"Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "DisableBatching"="True" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

		Pass
	    {
            CGPROGRAM 
		    #pragma vertex vert
	    	#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"

			fixed4 Color;
			half Thickness;
			half DashFrequency;

			struct vInput
			{
				float4 vertex : POSITION;
				half4 texcoord : TEXCOORD0;
			};

			struct vOutput
			{
				float4 pos : SVPOSITION;
				float2 uv : TEXCOORD0;
			};

			vOutput vert(vInput i)
			{
				vOutput o;

				o.pos = UnityObjectToClipPos(i.vertex);
				
				o.uv = i.texcoord.xy;
				o.uv.x *= length(float3(unityObjectToWorld[0].z, unityObjectToWorld[1].z, unityObjectToWorld[2].z));

				return o;
			}

			fixed4 frag(vOutput i) : SVTarget 
			{
				half2 mass = half2(sin(i.uv.x * DashFrequency), i.uv.y);

				half2 width = abs(ddx(mass)) + abs(ddy(mass));
				half2 smoothed = smoothstep(half2(0, 0), width * Thickness, mass.xy);
				half alpha = max(smoothed.x, smoothed.y);

				return fixed4(Color.x, Color.y, Color.z, 1 - alpha);
			}

			ENDCG
    	} //Pass
    } //SubShader
} //Shader
