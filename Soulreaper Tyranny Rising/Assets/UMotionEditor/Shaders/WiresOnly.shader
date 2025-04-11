Shader "UMotion Editor/Wires Only"
{ 
    Properties 
    {
		Color("Main Color (RGB)", color) = (1, 1, 1, 1)
		Size("Wire Size", Range(0, 4)) = 0.9
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
			half Size;

			struct vInput
			{
				float4 vertex : POSITION;
				half4 texcoord : TEXCOORD0;
			};

			struct vOutput
			{
				float4 pos : SVPOSITION;
				fixed3 wirecoord : TEXCOORD1;		
			};

			vOutput vert(vInput i)
			{
				vOutput o;

				o.pos = UnityObjectToClipPos(i.vertex);

				o.wirecoord = fixed3(floor(i.texcoord.x), frac(i.texcoord.x) * 10, i.texcoord.y);

				return o;
			}

			fixed4 frag(vOutput i) : SVTarget 
			{
				half3 width = abs(ddx(i.wirecoord.xyz)) + abs(ddy(i.wirecoord.xyz));
				half3 smoothed = smoothstep(half3(0, 0, 0), width * Size, i.wirecoord.xyz);		
	  			half wireAlpha = min(min(smoothed.x, smoothed.y), smoothed.z);

				return fixed4(Color.xyz, 1 - wireAlpha);
			}

			ENDCG
    	} //Pass
    } //SubShader
} //Shader
