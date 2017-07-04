Shader "Merlin/StencilHighlight" 
{
	Properties 
	{
		[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry-1" }
		LOD 200

		Pass
		{
			Stencil
			{
				Ref [_StencilRefValue]
				Comp always
				Pass replace
				ZFail replace
			}

			Name "HIGHLIGH_PREPARE"
			ColorMask 0 // Don't write to any colour channels
			ZWrite Off // Don't write to the Depth buffer
		}
	} 
}
