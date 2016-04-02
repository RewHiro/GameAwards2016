Shader "Transition/Transition"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_AlphaMask("Rule", 2D) = "white"{}
		_Range("Range", Range(0,2)) = 1
	}
		SubShader
	{
		// No culling or depth
		Cull Off
		ZWrite Off
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Tags
		{
			"Queue" = "AlphaTest"
			"RenderType" = "TransparentCutout"
			"PreviewType" = "Plane"
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			sampler2D _MainTex;
			fixed4 _Color;
			sampler2D _AlphaMask;
			float _Range;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				float alpha = tex2D(_AlphaMask, i.uv).a + (_Range - 1);
				alpha = clamp(alpha, 0, 1);
				col.a *= alpha;

				return col;
			}

			ENDCG
		}
	}
}
