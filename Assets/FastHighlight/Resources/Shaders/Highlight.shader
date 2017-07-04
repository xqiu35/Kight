// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Merlin/Highlight" 
{
	Properties 
	{
		[HideInInspector] _HighlightColor ("Highlight Color", Color) = (1,1,1,1)
		[HideInInspector] _HighlightThickness ("Highlight Thickness", Float) = 0.05
		[HideInInspector] _StencilRefValue ("Stencil Ref Value", Int) = 187
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" "Queue"="Transparent+500" }
		LOD 200

		Pass
		{
			Stencil
			{
				Ref [_StencilRefValue]
				Comp NotEqual
			}

			Name "HIGHLIGHT"
			Cull Back 
			ZTest Off
			ZWrite Off
			ColorMask RGB
			Tags { "LightMode" = "Always" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature FLUID_GEOMETRY

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			fixed4 _HighlightColor;
			float _HighlightThickness;
			float4 _HighlightEffectCenter;

			v2f vert(appdata_base v)
			{
				v2f o;

				#if FLUID_GEOMETRY
	    			o.vertex = v.vertex;
				    o.vertex.xyz += v.normal.xyz *_HighlightThickness * 0.4f;
				    o.vertex = UnityObjectToClipPos(o.vertex);
				#else
					float3 highlightOffset = normalize(v.vertex.xyz) * _HighlightThickness;
					o.vertex = UnityObjectToClipPos(v.vertex + float4(highlightOffset, 0));
				#endif

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return _HighlightColor;
			}

			ENDCG
		}
	} 
}
