Shader "Merlin/5.2.2f1/UI/Unlit/Text Detail"
{
	Properties
	{
		[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
		
		_DetailTex ("Detail (RGB)", 2D) = "white" {}
		_Strength ("Detail Strength", Range(0.0, 1.0)) = 0.2
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15
	}

	FallBack "UI/Unlit/Detail"
}
