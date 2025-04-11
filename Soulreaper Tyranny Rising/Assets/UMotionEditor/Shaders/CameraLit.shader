Shader "UMotion Editor/Camera Lit"
{ 
    Properties 
    {
		Color("Main Color (RGB)", color) = (1, 1, 1, 1)
		WireColor("Wire Color (RGB) Trans (A)", color) = (0, 0, 0, 1)	
		WireSize("Wire Size", Range(0, 4)) = 0.9
    }

    SubShader 
    {
		Tags { "RenderType" = "Opaque" "IgnoreProjector"="True" }
		LOD 100

		Pass
	    {
            CGPROGRAM 
		    #pragma vertex vert
	    	#pragma fragment frag
			#pragma target 3.0
			
		    #include "UnityCG.cginc"

			fixed4 Color;
			fixed4 WireColor;
			half WireSize;

			struct vInput
			{
				float4 vertex : POSITION;
				half4 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct vOutput
			{
				float4 pos : SVPOSITION;
				fixed3 wirecoord : TEXCOORD1;
				float3 lambert : TEXCOORD2;		
			};

			vOutput vert(vInput i)
			{
				vOutput o;

				o.pos = UnityObjectToClipPos(i.vertex);

				o.wirecoord = fixed3(floor(i.texcoord.x), frac(i.texcoord.x) * 10, i.texcoord.y);

				float3 viewDir = normalize(WorldSpaceViewDir(i.vertex));
				float3 worldNormal = normalize(UnityObjectToWorldNormal(i.normal));
				o.lambert = saturate(dot(viewDir, worldNormal));

				return o;
			}

			fixed4 frag(vOutput i) : SVTarget 
			{
				fixed4 outColor;
				outColor.rgb = i.lambert * Color;
				outColor.a = 1.0;
				 
				half3 width = abs(ddx(i.wirecoord.xyz)) + abs(ddy(i.wirecoord.xyz));
				half3 smoothed = smoothstep(half3(0, 0, 0), width * WireSize, i.wirecoord.xyz);		
	  			half wireAlpha = min(min(smoothed.x, smoothed.y), smoothed.z);	
				
				return lerp(lerp(outColor, WireColor, WireColor.a), outColor, wireAlpha);
			}

			ENDCG
    	} //Pass
    } //SubShader
} //Shader
