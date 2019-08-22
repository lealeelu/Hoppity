// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Particle/Additive and Alphablend"
{
	Properties
	{
		_MainTex("_MainTex RGBA", 2D) = "white" {}
		_ColorTintAdd("ColorTintAdd", Color) = (1,1,1,1)
		_ColorTintMult("ColorTintMult", Color) = (1,1,1,1)
	}

		Category
	{
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		 Cull Off Lighting Off ZWrite Off Lighting Off

		SubShader
		{
			Pass
			{
		//Render Alpha Blend First
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float4 _ColorTintMult;

		struct appdata_t {
			fixed4 vertex : POSITION;
			fixed2 texcoord : TEXCOORD0;
			fixed4 color : COLOR;
		};

		struct v2f {
			fixed4 vertex : SV_POSITION;
			fixed2 texcoord : TEXCOORD0;
			fixed4 color : COLOR;
		};

		fixed4 _MainTex_ST;

		v2f vert(appdata_t v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.color = v.color;
			o.color.a = _ColorTintMult.a;

			return o;
		}

		fixed4 frag(v2f i) : Color
		{
			fixed4 tex = tex2D(_MainTex, i.texcoord);

			return tex * i.color;
		}
		ENDCG
	}

	Pass
	{
			//Then Additive
			Blend SrcAlpha One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _ColorTintAdd;

			struct appdata_t {
				fixed4 vertex : POSITION;
				fixed2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f {
				fixed4 vertex : SV_POSITION;
				fixed2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			fixed4 _MainTex_ST;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.color = v.color;
				o.color.a = _ColorTintAdd.a;

				return o;
			}

			fixed4 frag(v2f i) : Color
			{
				fixed4 tex = tex2D(_MainTex, i.texcoord);

				return tex * i.color;
			}
			ENDCG
		}
	}
	}
}
