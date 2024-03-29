// Upgrade NOTE: commented out 'float4x4 _WorldToCamera', a built-in variable
// Upgrade NOTE: replaced '_WorldToCamera' with 'unity_WorldToCamera'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Merlin/5.2.2f1/Hidden/Internal-CombineDepthNormals" {
		Properties {[HideInInspector] _StencilRefValue ("StencilRefValue", Int) = 187 }
SubShader {Stencil{Ref [_StencilRefValue] Comp always Pass replace ZFail replace }
	
Pass {
	ZWrite Off ZTest Always Cull Off
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

struct appdata {
	float4 vertex : POSITION;
	float2 texcoord : TEXCOORD0;
};

struct v2f {
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
};
float4 _CameraNormalsTexture_ST;

v2f vert (appdata v)
{
	v2f o;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.uv = TRANSFORM_TEX(v.texcoord,_CameraNormalsTexture);
	return o;
}
sampler2D_float _CameraDepthTexture;
sampler2D _CameraNormalsTexture;

// float4x4 _WorldToCamera;

fixed4 frag (v2f i) : SV_Target
{
	float d = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
	float3 n = tex2D (_CameraNormalsTexture, i.uv) * 2.0 - 1.0;
	d = Linear01Depth (d);
	n = mul ((float3x3)unity_WorldToCamera, n);
	n.z = -n.z;
	return (d < (1.0-1.0/65025.0)) ? EncodeDepthNormal (d, n.xyz) : float4(0.5,0.5,1.0,1.0);
}
ENDCG
}

}
Fallback Off
}
