// Does not do anything in 3.x
Shader "Merlin/5.2.2f1/Legacy Shaders/Diffuse Fast" {
Properties {[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
Fallback "Legacy Shaders/VertexLit"
}
