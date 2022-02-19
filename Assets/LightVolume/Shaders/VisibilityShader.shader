Shader "VolLight/Visibility"
{

    SubShader
	{
		ZWrite Off
        ZTest Always
        Cull Off
        Lighting Off
        Fog { Mode Off }

        Tags { "Queue" = "Geometry+20" }

		Pass
		{
			//Blend Zero SrcAlpha 
			//Blend One One, One Zero

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

			fixed4 frag (v2f i) : SV_Target
			{
				return fixed4(0.2,0.0,0.0,1.0);
			}
			ENDCG
		}
	}

}
