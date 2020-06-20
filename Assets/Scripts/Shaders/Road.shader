Shader "Custom/Road"
{
	Properties
	{ 
		//[PerRendererData]
		_MainTex("Sprite Texture", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_Threshold("Threshold", Float) = 0.5
		_EdgeMask("Edge Mask", Float) = 0.5
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ PIXELSNAP_ON
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					float3 worldPos : TEXCOORD1;
				};

				fixed4 _Color;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex);
					OUT.color = IN.color * _Color;
					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					return OUT;
				}

				sampler2D _MainTex;
				float4  _MainTex_ST;

				sampler2D _Mask;
				float4  _Mask_ST;
				float _Threshold;
				float _EdgeMask;

				sampler2D _AlphaTex;
				float _AlphaSplitEnabled;

				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);

	#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
					if (_AlphaSplitEnabled)
						color.a = tex2D(_AlphaTex, uv).r;
	#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

					return color;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					float2 uv = frac(IN.worldPos.xy/_MainTex_ST.xy);
					fixed4 c = SampleSpriteTexture(uv) * IN.color;

					float2 muv = frac(IN.worldPos.xy / _Mask_ST.xy + _Mask_ST.zw);
					fixed m = tex2D(_Mask, muv).r * (1 - abs(IN.texcoord.y * 2 - 1) * _EdgeMask);
					clip(m-_Threshold);

					c.rgb *= c.a;
					return c;
				}
			ENDCG
			}
		}
}