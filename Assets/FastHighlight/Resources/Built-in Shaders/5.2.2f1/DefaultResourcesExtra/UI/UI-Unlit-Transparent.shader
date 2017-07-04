Shader "Merlin/5.2.2f1/UI/Unlit/Transparent"
{
	Properties
	{
		[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)

		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15
	}
	FallBack "UI/Default"
}
