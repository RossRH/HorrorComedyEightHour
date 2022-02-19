// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "VolLight/CutoutGrass" 
{
    Properties
    {
        _MainTex ("Base (RGB) Trans. (Alpha)", 2D) = "white" { }
		_Cutoff ("Alpha cutoff", Range (0,1)) = 1
		_WaveIntensity ("Wave Intensity", Range (0,10)) = 1
		[PerRendererData]_WaveOffset ("Wave Offset", Float) = 0
		[PerRendererData]_WaveIntensityIndividual ("Wave Intensity Individual", Float) = 0
	}

    SubShader
	{
        ZWrite On
        Cull Off
        Lighting Off
        ZTest Less
        

     //   Tags { "DisableBatching" = "True" }
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

            float _WaveIntensity;
            float _WaveIntensityIndividual;
            float _WaveOffset;
            
			v2f vert (appdata v)
			{
				v2f o;
				
				float3 baseWorldPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) ).xyz;
				
				//float phase = baseWorldPos.x * 80 + baseWorldPos.y * 80;				
				v.vertex.x += sin(_Time * 80 + _WaveOffset) * max(_WaveIntensity, _WaveIntensityIndividual) * clamp(v.uv.y * 1.3 - 0.3, 0, 1);
				
				o.vertex = UnityObjectToClipPos(v.vertex);
				
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _Cutoff;

			fixed4 frag (v2f i) : SV_Target
			{
			    fixed2 uvWave = i.uv;
				fixed4 col = tex2D(_MainTex, uvWave);
				clip(col.a == 0 ? -1 : 1);
		
				return col;// + fixed4(1.0, 0.0 , 0.0, 0.0);
			}
			ENDCG
		}
	} 
}