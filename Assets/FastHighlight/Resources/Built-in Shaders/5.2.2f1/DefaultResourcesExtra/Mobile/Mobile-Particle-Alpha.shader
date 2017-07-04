// Simplified Alpha Blended Particle shader. Differences from regular Alpha Blended Particle one:
// - no Tint color
// - no Smooth particle support
// - no AlphaTest
// - no ColorMask

Shader "Merlin/5.2.2f1/Mobile/Particles/Alpha Blended" {
Properties {[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187
	_MainTex ("Particle Texture", 2D) = "white" {}
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	SubShader {Stencil{Ref [_StencilRefValue] Comp always Pass replace ZFail replace }
		Pass {
			SetTexture [_MainTex] {
				combine texture * primary
			}
		}
	}
}
}
