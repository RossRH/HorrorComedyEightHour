Shader "VolLight/Cutout"
{
    Properties
    {
        _MainTex ("Base (RGB) Trans. (Alpha)", 2D) = "white" { }
		_Cutoff ("Alpha cutoff", Range (0,1)) = 1
		_Color("Color", Color) = (1,1,1,1)

		[PerRenderData]
		_OverrideColor("Override Color", Color) = (0, 0, 0, 0)
	}

    SubShader
	{
        ZWrite On
        Cull Off
        Lighting Off
        ZTest Less

     //   Tags { "Queue" = "Geometry+9" }
        Blend SrcAlpha OneMinusSrcAlpha, One One

		Pass
		{

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
			float _Cutoff;
			fixed4 _Color;
			fixed4 _OverrideColor;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				clip(col.a == 0 ? -1 : 1);

				float4 baseColor = col * _Color;
				fixed3 overriddenColor = (baseColor.rgb * (1-_OverrideColor.a)) + _OverrideColor.rgb * _OverrideColor.a; //replace the color with override color, lerping baseColor by override alpha to avoid clipping
				return fixed4(overriddenColor, baseColor.a);  //use base color alpha to preserve sprite alpha
			}
			ENDCG
		}
	}
}