Shader "Glow/GlowPrepassSprite"
{
      Properties
    {
        _MainTex ("Base (RGB) Trans. (Alpha)", 2D) = "white" { }
		_Cutoff ("Alpha cutoff", Range (0,1)) = 1
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
			
			fixed4 _GlowColor;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				clip(col.a == 0 ? -1 : 1);
		
				return _GlowColor;
			}
			ENDCG
		}
	} 
}