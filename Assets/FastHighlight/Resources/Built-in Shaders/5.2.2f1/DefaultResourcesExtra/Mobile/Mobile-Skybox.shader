// Simplified Skybox shader. Differences from regular Skybox one:
// - no tint color

Shader "Merlin/5.2.2f1/Mobile/Skybox" {
Properties {[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187
	_FrontTex ("Front (+Z)", 2D) = "white" {}
	_BackTex ("Back (-Z)", 2D) = "white" {}
	_LeftTex ("Left (+X)", 2D) = "white" {}
	_RightTex ("Right (-X)", 2D) = "white" {}
	_UpTex ("Up (+Y)", 2D) = "white" {}
	_DownTex ("Down (-Y)", 2D) = "white" {}
}

SubShader {Stencil{Ref [_StencilRefValue] Comp always Pass replace ZFail replace }
	Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
	Cull Off ZWrite Off Fog { Mode Off }
	Pass {
		SetTexture [_FrontTex] { combine texture }
	}
	Pass {
		SetTexture [_BackTex]  { combine texture }
	}
	Pass {
		SetTexture [_LeftTex]  { combine texture }
	}
	Pass {
		SetTexture [_RightTex] { combine texture }
	}
	Pass {
		SetTexture [_UpTex]    { combine texture }
	}
	Pass {
		SetTexture [_DownTex]  { combine texture }
	}
}
}
