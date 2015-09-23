Shader "MHImport/CustomShaderNoBump"{
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

   SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 200
         
 		Cull Off 
         CGPROGRAM 
         #pragma surface surf Lambert alphatest:_Cutoff

		sampler2D _MainTex;

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

Fallback "Transparent/VertexLit"
}
