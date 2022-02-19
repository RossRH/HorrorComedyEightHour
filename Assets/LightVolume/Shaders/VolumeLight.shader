Shader "VolLight/Light"
{

	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Tint ("Tint", Color) = (1, 1, 1, 1)
		_Intensity ("Intensity", Float) = 1
	}

    SubShader
	{
		ZWrite Off
        ZTest Always
        Cull Back
        Lighting Off
        Fog { Mode Off }

        Tags { "Queue" = "Geometry+21" }

		Pass
		{
			// WORKING: Blend DstAlpha One
			Blend One One, Zero One

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _Tint;
			float _Intensity;

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv) * _Tint * _Intensity;
				return col;
			}
			ENDCG
		}
	}

}
