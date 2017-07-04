// Simplified VertexLit Blended Particle shader. Differences from regular VertexLit Blended Particle one:
// - no AlphaTest
// - no ColorMask

Shader "Merlin/5.2.2f1/Mobile/Particles/VertexLit Blended" {
Properties {[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187
	_EmisColor ("Emissive Color", Color) = (.2,.2,.2,0)
	_MainTex ("Particle Texture", 2D) = "white" {}
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off ZWrite Off Fog { Color (0,0,0,0) }
	
	Lighting On
	Material { Emission [_EmisColor] }
	ColorMaterial AmbientAndDiffuse

	SubShader {Stencil{Ref [_StencilRefValue] Comp always Pass replace ZFail replace }
		Pass {
			SetTexture [_MainTex] {
				combine texture * primary
			}
		}
	}
}
}
