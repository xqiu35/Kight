// Simplified Multiply Particle shader. Differences from regular Multiply Particle one:
// - no Smooth particle support
// - no AlphaTest
// - no ColorMask

Shader "Merlin/5.2.2f1/Mobile/Particles/Multiply" {
Properties {[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187
	_MainTex ("Particle Texture", 2D) = "white" {}
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend Zero SrcColor
	Cull Off Lighting Off ZWrite Off Fog { Color (1,1,1,1) }
	
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
			SetTexture [_MainTex] {
				constantColor (1,1,1,1)
				combine previous lerp (previous) constant
			}
		}
	}
}
}
