// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

#ifndef CALMWATER_DX11_INCLUDED
#define CALMWATER_DX11_INCLUDED

#include "CalmWater_Helper.cginc"


#ifndef LIGHTCOLOR
#define LIGHTCOLOR
uniform fixed4 _LightColor0;
#endif

// ===========================================================================================

struct appdata {
    float4 vertex 	: POSITION;
    float3 normal 	: NORMAL;
    float4 tangent 	: TANGENT;
    float2 texcoord : TEXCOORD0;
    float4 color 	: COLOR;
};


// V2F
struct v2f {
    float4 pos 			: SV_POSITION;
    fixed3 ambient  	: COLOR;

    half4 tspace0 : TEXCOORD0; // tangent.x, bitangent.x, normal.x
    half4 tspace1 : TEXCOORD1; // tangent.y, bitangent.y, normal.y
    half4 tspace2 : TEXCOORD2; // tangent.z, bitangent.z, normal.z

    float3 worldPos : TEXCOORD3;
    float4 GrabUV 	: TEXCOORD4;
    float4 DepthUV	: TEXCOORD5;
    float4 AnimUV	: TEXCOORD6;

    SHADOW_COORDS(7)

    #ifdef UNITY_PASS_FORWARDBASE
    UNITY_FOG_COORDS(8)
    #endif

    #ifndef SHADER_API_D3D9
    #if _FOAM_ON
    float2 FoamUV	: TEXCOORD9;
    #endif
    #endif
};

void displacement (inout appdata v)
{

	half4 worldSpaceVertex 	= mul(unity_ObjectToWorld,(v.vertex));
	half3 offsets;
	half3 nrml;

	#if _DISPLACEMENTMODE_WAVE
		Wave (
			offsets, nrml, v.vertex.xyz,worldSpaceVertex,
			_Amplitude,
			_Frequency,
			_Speed
		);
		v.vertex.y 		+= offsets.y;
		v.normal 		= nrml;
	#endif

	#if _DISPLACEMENTMODE_GERSTNER
		half3 vtxForAni 		= (worldSpaceVertex.xyz).xzz; // REMOVE VARIABLE
		Gerstner (
			offsets, nrml, v.vertex.xyz, vtxForAni,				// offsets, nrml will be written
			_Amplitude * 0.01,									// amplitude
			_Frequency,											// frequency
			_Steepness,											// steepness
			_WSpeed,											// speed
			_WDirectionAB,										// direction # 1, 2
			_WDirectionCD										// direction # 3, 4									
		);

		v.vertex.xyz += offsets;
		v.normal = nrml;
	#endif
}

// Vertex
v2f vert (appdata v) {
    //v2f o = (v2f)0;

    v2f o;

	#if !_DISPLACEMENTMODE_OFF
		displacement(v);
	#endif

    o.pos 			= UnityObjectToClipPos(v.vertex);
    o.GrabUV 		= ComputeGrabScreenPos(o.pos);
    o.DepthUV 		= ComputeScreenPos(o.pos);
	COMPUTE_EYEDEPTH(o.DepthUV.z);

	//UV Animation
	o.AnimUV = 	AnimateBump(v.texcoord);

	//Foam
	#ifndef SHADER_API_D3D9
	#if _FOAM_ON
	o.FoamUV =	TRANSFORM_TEX(v.texcoord,_FoamTex);
	#endif
	#endif

	//Normals
	float4 worldPos 	= mul(unity_ObjectToWorld, v.vertex);
	fixed3 worldNormal 	= UnityObjectToWorldNormal(v.normal);
	fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
	fixed tangentSign 	= v.tangent.w * unity_WorldTransformParams.w;
	fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;

	o.tspace0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
	o.tspace1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
	o.tspace2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);
	o.worldPos = worldPos;

	o.ambient = ShadeSH9(half4(worldNormal,1));

	#ifdef UNITY_PASS_FORWARDBASE
	UNITY_TRANSFER_FOG(o,o.pos);
	#endif
	
	TRANSFER_SHADOW(o)

    return o;
}



// ============================================
// Frag
// ============================================
fixed4 frag( v2f i ) : SV_Target
{
	// =========================================
	// Directions
	// =========================================

	// NormalMaps
	fixed4 n1			= tex2D(_BumpMap, i.AnimUV.xy);
	fixed4 n2 			= tex2D(_BumpMap, i.AnimUV.zw);
	fixed3 finalBump 	= UnpackNormalBlend(n1,n2, _BumpStrength);

	// World Normal
	fixed3 worldN = WorldNormal(i.tspace0.xyz, i.tspace1.xyz, i.tspace2.xyz, finalBump);

	// World ViewDir
	float3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
	
	#ifdef CULL_FRONT
		worldViewDir = -worldViewDir;
	#endif

	// World LightDir
	#ifndef USING_DIRECTIONAL_LIGHT
		fixed3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
	#else
		fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
	#endif

	// ========================================
	// Textures
	// ========================================

	float2 offset = worldN.xz * _GrabTexture_TexelSize.xy * _Distortion;

	// Depth Distortion ===================================================
	float4 DepthUV 	= OffsetUV(i.DepthUV,offset);
	// GrabPass Distortion ================================================
	float4 GrabUV 	= OffsetUV(i.GrabUV,offset);
	
	// Refraction ============================================================
	// RGB 	= Color
	// A 	= Depth
	// =======================================================================
	half4 refraction 		= tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(GrabUV));
	half4 cleanRefraction  	= tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.GrabUV));
	
	//Depth Texture Clean
	float sceneZ 	= texDepth(_CameraDepthTexture,i.DepthUV);
	//Depth Texture Distorted
	float DistZ 	= texDepth(_CameraDepthTexture,DepthUV);

	//Depth	
	refraction.a 		= saturate(_Depth / abs(DistZ - DepthUV.z));
	//Clean Depth
	cleanRefraction.a 	= saturate(_Depth / abs(sceneZ - i.DepthUV.z));

	//TODO: Remove keyword in cull front pass and remove check CULL_FRONT here
	#ifndef CULL_FRONT
		#if _DISTORTIONQUALITY_HIGH 
		//Hide refraction from objects over the surface		
		refraction 	= lerp(cleanRefraction,refraction,step(DepthUV.z, DistZ));           
		#endif
	#endif

	//Final color with depth and refraction
	#ifndef CULL_FRONT
		#if _DEPTHFOG_ON
			fixed3 finalColor = lerp(_Color.rgb * refraction.rgb,_DepthColor.rgb, 1.0 - refraction.a);
		#else
			fixed3 finalColor = lerp(_Color.rgb,_DepthColor.rgb, 1.0 - refraction.a) * refraction.rgb;
		#endif
	#else
		fixed3 finalColor = lerp(_Color.rgb,_DepthColor.rgb, 0.5) * refraction.rgb;
	#endif

	// =====================================================================
	// Reflections 
	// No Reflection on backface
	// =====================================================================
	#ifndef CULL_FRONT

		//Reverse cubeMap Y to look like reflection
		#if _REFLECTIONTYPE_MIXED || _REFLECTIONTYPE_CUBEMAP
		half3 worldRefl 	= reflect(-worldViewDir, worldN);
		fixed3 cubeMap 		= texCUBE(_Cube, worldRefl).rgb * _CubeColor.rgb;
		#endif

		#if _REFLECTIONTYPE_MIXED || _REFLECTIONTYPE_REALTIME
		//Real Time reflections

		//TODO: Upgrade to GrabUV when unity fixes its bug
		fixed3 rtReflections = tex2Dproj(_ReflectionTex, UNITY_PROJ_COORD(DepthUV)) * _Reflection;
		#endif

		#if _REFLECTIONTYPE_MIXED
		fixed3 finalReflection = lerp(cubeMap,rtReflections, 0.5);
		#endif

		#if _REFLECTIONTYPE_REALTIME
		fixed3 finalReflection = rtReflections;
		#endif

		#if _REFLECTIONTYPE_CUBEMAP
		fixed3 finalReflection = cubeMap;
		#endif
	//end CULL_FRONT
	#endif 

	//FOAM ======================================================================

	#if _FOAM_ON

		#ifndef SHADER_API_D3D9
		float2 foamUV = i.FoamUV;
		#else
		float2 foamUV = i.worldPos.xz;
		#endif
		
		//Foam Texture with animation
		fixed foamTex = tex2D(_FoamTex,foamUV + (finalBump.xy * 0.02)).r;
		//Final Foam 
		fixed3 foam = 1.0 - saturate(_FoamSize * (sceneZ-DepthUV.z) );//saturate((1.0 - abs(_FoamSize * cleanDepth)));
		foam *= _FoamColor.rgb * foamTex * max(_LightColor0.rgb ,i.ambient.rgb);

		finalColor		+= min(1.0,2.0 * foam);
	#endif

	// Terms =====================================================================
	
	// Atten
	UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos)
	// NdotL
	fixed NdotL 	= DiffuseTerm(worldN,lightDir);// + i.ambient * atten;

	//Apply Reflections
	#ifndef CULL_FRONT
	fixed NdotV 	= saturate( 1.0 - NdotVTerm(worldN,worldViewDir));
	half fresnel 	= pow(NdotV,_RimPower);
	finalColor 		= lerp(finalColor,finalReflection,fresnel * _Reflection);
	#endif
	
	//Add Lights
	#ifdef UNITY_PASS_FORWARDADD
	finalColor += NdotL * _LightColor0.rgb;
	#endif
	
	//Specular
	fixed3 specColor 	= SpecularColor (_Smoothness,lightDir,worldViewDir,worldN);

	//Albedo
	fixed3 diff 		= (finalColor * max(i.ambient,NdotL) * atten + specColor);

	//Alpha
	fixed alpha			= saturate(_EdgeFade * (sceneZ-DepthUV.z) ) * _Color.a;

	fixed4 c;

	#ifndef UNITY_PASS_FORWARDADD
		c.rgb 	= lerp(cleanRefraction.rgb,diff,alpha);
		UNITY_APPLY_FOG(i.fogCoord, c);
	#else
    	c.rgb 	= (diff * alpha) * atten;
	#endif

	c.a 	= 1;


	return c;

}

#ifdef DX11
#ifdef UNITY_CAN_COMPILE_TESSELLATION
	struct TessVertex {
		float4 vertex 	: INTERNALTESSPOS;
		float3 normal 	: NORMAL;
		float4 tangent 	: TANGENT;
		float2 texcoord : TEXCOORD0;
		//float4 color 	: COLOR;
	};

	struct OutputPatchConstant {
		float edge[3]         : SV_TessFactor;
		float inside          : SV_InsideTessFactor;
	};
	TessVertex tessvert (appdata v) {
		TessVertex o;
		o.vertex 	= v.vertex;
		o.normal 	= v.normal;
		o.tangent 	= v.tangent;
		o.texcoord 	= v.texcoord;
		//o.color 	= v.color;
		return o;
	}

    float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
        return UnityEdgeLengthBasedTess(v.vertex, v1.vertex, v2.vertex, 32 - _Tess);
    }

    OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
        OutputPatchConstant o;
        float4 ts = Tessellation( v[0], v[1], v[2] );
        o.edge[0] = ts.x;
        o.edge[1] = ts.y;
        o.edge[2] = ts.z;
        o.inside = ts.w;
        return o;
    }

    [domain("tri")]
    [partitioning("fractional_odd")]
    [outputtopology("triangle_cw")]
    [patchconstantfunc("hullconst")]
    [outputcontrolpoints(3)]
    TessVertex hs_surf (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
        return v[id];
    }

    [domain("tri")]
    v2f ds_surf (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
        appdata v = (appdata)0;

        v.vertex 	= vi[0].vertex*bary.x 	+ vi[1].vertex*bary.y 	+ vi[2].vertex*bary.z;
        v.texcoord 	= vi[0].texcoord*bary.x + vi[1].texcoord*bary.y + vi[2].texcoord*bary.z;
        //v.color 	= vi[0].color*bary.x 	+ vi[1].color*bary.y 	+ vi[2].color*bary.z;
        v.tangent 	= vi[0].tangent*bary.x 	+ vi[1].tangent*bary.y 	+ vi[2].tangent*bary.z;
        v.normal 	= vi[0].normal*bary.x 	+ vi[1].normal*bary.y  	+ vi[2].normal*bary.z;

        //float3 vertNormals = abs(v.normal);


        v2f o = vert(v);


        return o;
    }

#endif
#endif

#endif