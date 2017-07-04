Shader "Merlin/5.2.2f1/Hidden/TerrainEngine/Splatmap/Specular-Base" {
	Properties {[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}

		// used in fallback on old cards
		_Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader {Stencil{Ref [_StencilRefValue] Comp always Pass replace ZFail replace } 
		Tags {
			"RenderType" = "Opaque"
			"Queue" = "Geometry-100"
		}
		LOD 200

		CGPROGRAM
		#pragma surface surf BlinnPhong
		
		sampler2D _MainTex;
		half _Shininess;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = tex.rgb;
			o.Gloss = tex.a;
			o.Alpha = 1.0f;
			o.Specular = _Shininess;
		}
		ENDCG
	}

	FallBack "Legacy Shaders/Specular"
}
