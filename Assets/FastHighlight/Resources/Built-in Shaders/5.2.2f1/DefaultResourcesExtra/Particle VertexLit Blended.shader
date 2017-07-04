Shader "Merlin/5.2.2f1/Particles/VertexLit Blended" {
Properties {[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187
	_EmisColor ("Emissive Color", Color) = (.2,.2,.2,0)
	_MainTex ("Particle Texture", 2D) = "white" {}
}

SubShader {Stencil{Ref [_StencilRefValue] Comp always Pass replace ZFail replace }
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Tags { "LightMode" = "Vertex" }
	Cull Off
	Lighting On
	Material { Emission [_EmisColor] }
	ColorMaterial AmbientAndDiffuse
	ZWrite Off
	ColorMask RGB
	Blend SrcAlpha OneMinusSrcAlpha
	Pass {
		SetTexture [_MainTex] { combine primary * texture }
	}
}
}
