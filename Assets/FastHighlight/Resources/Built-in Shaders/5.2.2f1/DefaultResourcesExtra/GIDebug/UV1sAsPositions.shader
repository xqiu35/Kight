﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Merlin/5.2.2f1/Hidden/GIDebug/UV1sAsPositions" {
Properties {[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187
	_Color ("Main Color", Color) = (1,1,1,1)
}

SubShader {Stencil{Ref [_StencilRefValue] Comp always Pass replace ZFail replace }
	Tags { "RenderType"="Opaque" }
	LOD 100
	
	Pass { 
		ZTest Always
		Cull Off
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
			};
			
			struct v2f {
				float4 pos : SV_POSITION;
				float4 dummy : TEXCOORD0;
			};

			fixed4 _Color;
			float _StaticUV1;

			v2f vert (appdata v)
			{
				v2f o;
				float2 uv;
				if (_StaticUV1)
					uv = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				else
					uv = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				o.pos = UnityObjectToClipPos (float4 (uv, 0, 1));
				o.dummy = v.vertex; // make OpenGL happy
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				return _Color;
			}
		ENDCG
	}
}
}