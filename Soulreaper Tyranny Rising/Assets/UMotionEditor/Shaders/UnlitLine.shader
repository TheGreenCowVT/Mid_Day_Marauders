Shader "UMotion Editor/Unlit Line"
{ 
    Properties 
    {
		Color("Line Color (RGB) Trans (A)", color) = (0, 0, 0, 1)
		Thickness("Line Thikness", Range(0, 4)) = 0.9
    }

    SubShader 
    {
		Tags {"Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
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

			struct vInput
			{
				float4 vertex : POSITION;
				half4 texcoord : TEXCOORD0;
			};

			struct vOutput
			{
				float4 pos : SVPOSITION;
				float uvy : TEXCOORD0;
			};

			vOutput vert(vInput i)
			{
				vOutput o;

				o.pos = UnityObjectToClipPos(i.vertex);
				o.uvy = i.texcoord.y;

				return o;
			}

			fixed4 frag(vOutput i) : SVTarget 
			{
				half width = abs(ddx(i.uvy)) + abs(ddy(i.uvy));
				half alpha = smoothstep(0, width * Thickness, i.uvy);

				return fixed4(Color.x, Color.y, Color.z, 1 - alpha);
			}

			ENDCG
    	} //Pass
    } //SubShader
} //Shader
