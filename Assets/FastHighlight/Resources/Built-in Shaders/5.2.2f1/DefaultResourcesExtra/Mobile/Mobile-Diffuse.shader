// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Merlin/5.2.2f1/Mobile/Diffuse" {
Properties {[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {Stencil{Ref [_StencilRefValue] Comp always Pass replace ZFail replace }
	Tags { "RenderType"="Opaque" }
	LOD 150

CGPROGRAM
#pragma surface surf Lambert noforwardadd

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

Fallback "Mobile/VertexLit"
}
