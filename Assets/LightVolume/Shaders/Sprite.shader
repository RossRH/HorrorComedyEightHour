Shader "VolLight/Sprite"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Tint ("Tint", Color) = (1, 1, 1, 1)
	}

    SubShader
	{
        ZWrite On
        ZTest Less
        Cull Back
        Lighting Off

        Tags { "Queue" = "Geometry+10" }

		Pass
		{
			//Blend DstColor Zero

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			float4 _MainTex_ST;

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityPixelSnap(UnityObjectToClipPos(v.vertex));
				o.uv = TRANSFORM_TEX (v.uv, _MainTex);
			
				return o;
			}
			
			sampler2D _MainTex;
			fixed4 _Tint;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _Tint;
				return col;
			}
			ENDCG
		}
	}
}
