Shader "Merlin/5.2.2f1/Hidden/TerrainEngine/Details/Vertexlit" {
Properties {[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187
	_MainTex ("Main Texture", 2D) = "white" {  }
}
SubShader {Stencil{Ref [_StencilRefValue] Comp always Pass replace ZFail replace }
	Tags { "RenderType"="Opaque" }
	LOD 200

CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;

struct Input {
	float2 uv_MainTex;
	fixed4 color : COLOR;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}

ENDCG
}
SubShader {Stencil{Ref [_StencilRefValue] Comp always Pass replace ZFail replace }
	Tags { "RenderType"="Opaque" }
	Pass {
		Tags { "LightMode" = "Vertex" }
		ColorMaterial AmbientAndDiffuse
		Lighting On
		SetTexture [_MainTex] {
			constantColor (1,1,1,1)
			combine texture * primary DOUBLE, constant // UNITY_OPAQUE_ALPHA_FFP
		} 
	}
	Pass {
		Tags { "LightMode" = "VertexLMRGBM" }
		ColorMaterial AmbientAndDiffuse
		BindChannels {
			Bind "Vertex", vertex
			Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
			Bind "texcoord", texcoord1 // main uses 1st uv
		}
		SetTexture [unity_Lightmap] {
			matrix [unity_LightmapMatrix]
			combine texture * texture alpha DOUBLE
		}
		SetTexture [_MainTex] {
			combine texture * previous QUAD, constant // UNITY_OPAQUE_ALPHA_FFP
		}
	}
}

Fallback "VertexLit"
}
