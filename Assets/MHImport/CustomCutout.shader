Shader "MHImport/CustomCutoutShader" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}

SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 200
	


Cull Front // first pass renders only back faces 
// (the "inside")
ZWrite Off // don't write to depth buffer 
// in order not to occlude other objects
Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
CGPROGRAM
#pragma surface surf Lambert alpha

sampler2D _MainTex;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = c.rgb;
	o.Alpha = c.a*2.0;
}
ENDCG
Cull Back // first pass renders only back faces 
     // (the "inside")
 ZWrite Off // don't write to depth buffer 
    // in order not to occlude other objects
 Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
CGPROGRAM
#pragma surface surf Lambert alpha

sampler2D _MainTex;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = c.rgb;
	o.Alpha = c.a*2.0;
}
ENDCG


ZWrite On 

Blend Off // use alpha blending
 
 Cull Off 
CGPROGRAM
#pragma surface surf Lambert alphatest:_Cutoff

sampler2D _MainTex;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Transparent/Cutout/VertexLit"
}

