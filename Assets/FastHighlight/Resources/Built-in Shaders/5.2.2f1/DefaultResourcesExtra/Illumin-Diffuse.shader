// Upgrade NOTE: replaced 'defined UNITY_PASS_META' with 'defined (UNITY_PASS_META)'

Shader "Merlin/5.2.2f1/Legacy Shaders/Self-Illumin/Diffuse" {
Properties {[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Illum ("Illumin (A)", 2D) = "white" {}
	_Emission ("Emission (Lightmapper)", Range (0.0, 8.0)) = 1.0
}
SubShader {Stencil{Ref [_StencilRefValue] Comp always Pass replace ZFail replace }
	Tags { "RenderType"="Opaque" }
	LOD 200
	
CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
sampler2D _Illum;
fixed4 _Color;
fixed _Emission;

struct Input {
	float2 uv_MainTex;
	float2 uv_Illum;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 c = tex * _Color;
	o.Albedo = c.rgb;
	o.Emission = c.rgb * tex2D(_Illum, IN.uv_Illum).a;
#if defined (UNITY_PASS_META)
	o.Emission *= _Emission.rrr;
#endif
	o.Alpha = c.a;
}
ENDCG
} 
FallBack "Legacy Shaders/Self-Illumin/VertexLit"
CustomEditor "LegacyIlluminShaderGUI"
}