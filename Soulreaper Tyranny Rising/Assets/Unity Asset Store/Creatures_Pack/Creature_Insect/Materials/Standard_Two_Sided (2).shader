Shader "Standard Double Sided"
{
	Properties
	{
		Color("Color", Color) = (1,1,1,1)
		MainTex("Albedo", 2D) = "white" {}
		
		Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
		[Enum(Metallic Alpha,0,Albedo Alpha,1)] SmoothnessTextureChannel ("Smoothness texture channel", Float) = 0

		[Gamma] Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		MetallicGlossMap("Metallic", 2D) = "white" {}

		[ToggleOff] SpecularHighlights("Specular Highlights", Float) = 1.0
		[ToggleOff] GlossyReflections("Glossy Reflections", Float) = 1.0

		BumpScale("Scale", Float) = 1.0
		BumpMap("Normal Map", 2D) = "bump" {}

		Parallax ("Height Scale", Range (0.005, 0.08)) = 0.02
		ParallaxMap ("Height Map", 2D) = "black" {}

		OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		OcclusionMap("Occlusion", 2D) = "white" {}

		EmissionColor("Color", Color) = (0,0,0)
		EmissionMap("Emission", 2D) = "white" {}
		
		DetailMask("Detail Mask", 2D) = "white" {}

		DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
		DetailNormalMapScale("Scale", Float) = 1.0
		DetailNormalMap("Normal Map", 2D) = "bump" {}

		[Enum(UV0,0,UV1,1)] UVSec ("UV Set for secondary textures", Float) = 0


		// Blending state
		[HideInInspector] Mode ("mode", Float) = 0.0
		[HideInInspector] SrcBlend ("src", Float) = 1.0
		[HideInInspector] DstBlend ("dst", Float) = 0.0
		[HideInInspector] ZWrite ("zw", Float) = 1.0
	}

	CGINCLUDE
		#define UNITYSETUPBRDFINPUT MetallicSetup
	ENDCG

	SubShader
	{
		Tags { "RenderType"="Opaque" "PerformanceChecks"="False" }
		LOD 300
		Cull off

		// ------------------------------------------------------------------
		//  Base forward pass (directional light, emission, lightmaps, ...)
		Pass
		{
			Name "FORWARD" 
			Tags { "LightMode" = "ForwardBase" }

			Blend [SrcBlend] [DstBlend]
			ZWrite [ZWrite]

			CGPROGRAM
			#pragma target 3.0

			// -------------------------------------

			#pragma shaderfeature NORMALMAP
			#pragma shaderfeature  ALPHATESTON ALPHABLENDON ALPHAPREMULTIPLYON
			#pragma shaderfeature EMISSION
			#pragma shaderfeature METALLICGLOSSMAP
			#pragma shaderfeature  DETAILMULX2
			#pragma shaderfeature  SMOOTHNESSTEXTUREALBEDOCHANNELA
			#pragma shaderfeature  SPECULARHIGHLIGHTSOFF
			#pragma shaderfeature  GLOSSYREFLECTIONSOFF
			#pragma shaderfeature PARALLAXMAP

			#pragma multicompilefwdbase
			#pragma multicompilefog

			#pragma vertex vertBase
			#pragma fragment fragBase
			#include "UnityStandardCoreForward.cginc"

			ENDCG
		}
		// ------------------------------------------------------------------
		//  Additive forward pass (one light per pass)
		Pass
		{
			Name "FORWARDDELTA"
			Tags { "LightMode" = "ForwardAdd" }
			Blend [SrcBlend] One
			Fog { Color (0,0,0,0) } // in additive pass fog should be black
			ZWrite Off
			ZTest LEqual

			CGPROGRAM
			#pragma target 3.0

			// -------------------------------------


			#pragma shaderfeature NORMALMAP
			#pragma shaderfeature  ALPHATESTON ALPHABLENDON ALPHAPREMULTIPLYON
			#pragma shaderfeature METALLICGLOSSMAP
			#pragma shaderfeature  SMOOTHNESSTEXTUREALBEDOCHANNELA
			#pragma shaderfeature  SPECULARHIGHLIGHTSOFF
			#pragma shaderfeature  DETAILMULX2
			#pragma shaderfeature PARALLAXMAP

			#pragma multicompilefwdaddfullshadows
			#pragma multicompilefog


			#pragma vertex vertAdd
			#pragma fragment fragAdd
			#include "UnityStandardCoreForward.cginc"

			ENDCG
		}
		// ------------------------------------------------------------------
		//  Shadow rendering pass
		Pass {
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }

			ZWrite On ZTest LEqual

			CGPROGRAM
			#pragma target 3.0

			// -------------------------------------


			#pragma shaderfeature  ALPHATESTON ALPHABLENDON ALPHAPREMULTIPLYON
			#pragma multicompileshadowcaster

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#include "UnityStandardShadow.cginc"

			ENDCG
		}
		// ------------------------------------------------------------------
		//  Deferred pass
		Pass
		{
			Name "DEFERRED"
			Tags { "LightMode" = "Deferred" }

			CGPROGRAM
			#pragma target 3.0
			#pragma excluderenderers nomrt


			// -------------------------------------

			#pragma shaderfeature NORMALMAP
			#pragma shaderfeature  ALPHATESTON ALPHABLENDON ALPHAPREMULTIPLYON
			#pragma shaderfeature EMISSION
			#pragma shaderfeature METALLICGLOSSMAP
			#pragma shaderfeature  SMOOTHNESSTEXTUREALBEDOCHANNELA
			#pragma shaderfeature  SPECULARHIGHLIGHTSOFF
			#pragma shaderfeature  DETAILMULX2
			#pragma shaderfeature PARALLAXMAP

			#pragma multicompile  UNITYHDRON
			#pragma multicompile LIGHTMAPOFF LIGHTMAPON
			#pragma multicompile  DIRLIGHTMAPCOMBINED DIRLIGHTMAPSEPARATE
			#pragma multicompile DYNAMICLIGHTMAPOFF DYNAMICLIGHTMAPON

			#pragma vertex vertDeferred
			#pragma fragment fragDeferred

			#include "UnityStandardCore.cginc"

			ENDCG
		}

		// ------------------------------------------------------------------
		// Extracts information for lightmapping, GI (emission, albedo, ...)
		// This pass it not used during regular rendering.
		Pass
		{
			Name "META" 
			Tags { "LightMode"="Meta" }

			Cull Off

			CGPROGRAM
			#pragma vertex vertmeta
			#pragma fragment fragmeta

			#pragma shaderfeature EMISSION
			#pragma shaderfeature METALLICGLOSSMAP
			#pragma shaderfeature  SMOOTHNESSTEXTUREALBEDOCHANNELA
			#pragma shaderfeature  DETAILMULX2

			#include "UnityStandardMeta.cginc"
			ENDCG
		}
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" "PerformanceChecks"="False" }
		LOD 150

		// ------------------------------------------------------------------
		//  Base forward pass (directional light, emission, lightmaps, ...)
		Pass
		{
			Name "FORWARD" 
			Tags { "LightMode" = "ForwardBase" }

			Blend [SrcBlend] [DstBlend]
			ZWrite [ZWrite]

			CGPROGRAM
			#pragma target 2.0
			
			#pragma shaderfeature NORMALMAP
			#pragma shaderfeature  ALPHATESTON ALPHABLENDON ALPHAPREMULTIPLYON
			#pragma shaderfeature EMISSION 
			#pragma shaderfeature METALLICGLOSSMAP 
			#pragma shaderfeature  SMOOTHNESSTEXTUREALBEDOCHANNELA
			#pragma shaderfeature  SPECULARHIGHLIGHTSOFF
			#pragma shaderfeature  GLOSSYREFLECTIONSOFF
			// SM2.0: NOT SUPPORTED shaderfeature  DETAILMULX2
			// SM2.0: NOT SUPPORTED shaderfeature PARALLAXMAP

			#pragma skipvariants SHADOWSSOFT DIRLIGHTMAPCOMBINED DIRLIGHTMAPSEPARATE

			#pragma multicompilefwdbase
			#pragma multicompilefog

			#pragma vertex vertBase
			#pragma fragment fragBase
			#include "UnityStandardCoreForward.cginc"

			ENDCG
		}
		// ------------------------------------------------------------------
		//  Additive forward pass (one light per pass)
		Pass
		{
			Name "FORWARDDELTA"
			Tags { "LightMode" = "ForwardAdd" }
			Blend [SrcBlend] One
			Fog { Color (0,0,0,0) } // in additive pass fog should be black
			ZWrite Off
			ZTest LEqual
			
			CGPROGRAM
			#pragma target 2.0

			#pragma shaderfeature NORMALMAP
			#pragma shaderfeature  ALPHATESTON ALPHABLENDON ALPHAPREMULTIPLYON
			#pragma shaderfeature METALLICGLOSSMAP
			#pragma shaderfeature  SMOOTHNESSTEXTUREALBEDOCHANNELA
			#pragma shaderfeature  SPECULARHIGHLIGHTSOFF
			#pragma shaderfeature  DETAILMULX2
			// SM2.0: NOT SUPPORTED shaderfeature PARALLAXMAP
			#pragma skipvariants SHADOWSSOFT
			
			#pragma multicompilefwdaddfullshadows
			#pragma multicompilefog
			
			#pragma vertex vertAdd
			#pragma fragment fragAdd
			#include "UnityStandardCoreForward.cginc"

			ENDCG
		}
		// ------------------------------------------------------------------
		//  Shadow rendering pass
		Pass {
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			
			ZWrite On ZTest LEqual

			CGPROGRAM
			#pragma target 2.0

			#pragma shaderfeature  ALPHATESTON ALPHABLENDON ALPHAPREMULTIPLYON
			#pragma skipvariants SHADOWSSOFT
			#pragma multicompileshadowcaster

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#include "UnityStandardShadow.cginc"

			ENDCG
		}

		// ------------------------------------------------------------------
		// Extracts information for lightmapping, GI (emission, albedo, ...)
		// This pass it not used during regular rendering.
		Pass
		{
			Name "META" 
			Tags { "LightMode"="Meta" }

			Cull Off

			CGPROGRAM
			#pragma vertex vertmeta
			#pragma fragment fragmeta

			#pragma shaderfeature EMISSION
			#pragma shaderfeature METALLICGLOSSMAP
			#pragma shaderfeature  SMOOTHNESSTEXTUREALBEDOCHANNELA
			#pragma shaderfeature  DETAILMULX2

			#include "UnityStandardMeta.cginc"
			ENDCG
		}
	}


	FallBack "VertexLit"
	CustomEditor "StandardShaderGUI"
}
