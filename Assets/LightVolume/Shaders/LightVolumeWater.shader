Shader "VolLight/Water"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Tint ("Tint", Color) = (1, 1, 1, 1)
		_Intensity ("Intensity", Float) = 0.32
		_Intensity2 ("Intensity2", Float) = 0.32
		_Intensity3 ("Intensity3", Float) = 0.32
		_Speed ("Speed", Float) = 1.0
		_XMotion ("XMotion", Float) = 1.0
		_YMotion ("YMotion", Float) = 1.0
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
				float4 worldPos : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityPixelSnap(UnityObjectToClipPos(v.vertex));
				o.uv = TRANSFORM_TEX (v.uv, _MainTex);
				o.worldPos = mul (unity_ObjectToWorld, v.vertex);
			
				return o;
			}
			
			sampler2D _MainTex;
			fixed4 _Tint;
			float _Intensity;
			float _Intensity2;
			float _Intensity3;
			float _XMotion;
			float _YMotion;
			float _Speed;

			fixed4 frag (v2f i) : SV_Target
			{
				//float2 delta = float2(600, 600) - i.worldPos.xy;

				//delta = normalize(delta);

				//clip(delta.y > 0 ? 1 : -1);
				//clip(delta.x > 0.65 ? 1 : -1);


				float sinTerm = sin(_Time * _Speed + _XMotion * i.worldPos.x + _YMotion * i.worldPos.y);

				float4 baseCol = tex2D(_MainTex, i.uv  * (1.0 + _Intensity2 * sinTerm)) * _Tint;

				float waveShade = 0.5 - sinTerm * 0.5;
				fixed4 shadedCol = lerp(baseCol, float4(1,1,1,1), waveShade * 0.03);

				fixed4 col = lerp(shadedCol, float4(1,1,1,1), pow(waveShade,12) * 0.3);

				//return float4(delta.x, delta.y, 0, 1);
				return shadedCol;
			}
			ENDCG
		}
	}
}
