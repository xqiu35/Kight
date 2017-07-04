// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Merlin/5.2.2f1/Hidden/InternalErrorShader"
{
	Properties {[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187 }
	SubShader
	{
		Stencil{Ref [_StencilRefValue] Comp always Pass replace ZFail replace } 
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			float4 vert (float4 pos : POSITION) : SV_POSITION { return UnityObjectToClipPos(pos); }
			fixed4 frag () : COLOR { return fixed4(1,0,1,1); }
			ENDCG
		}
	}
	Fallback Off
}
