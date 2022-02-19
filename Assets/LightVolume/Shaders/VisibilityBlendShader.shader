Shader "Custom/VisibilityBlendShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_VisTex("Visibility Texture", 2D) = "white" {}
		_FishTankVisTex("Visibility Fish tank Texture", 2D) = "white" {}
		_EntityTex("Entity Texture", 2D) = "white" {}
		_VisEntityTex("Visibile Entity Texture", 2D) = "white" {}
		//_HighlightTex("Highlight Entity Texture", 2D) = "white" {}
		_LightTex0("Light Textures", 2D) = "black" {}

		_LightPos0 ("Light Pos 0", Vector) = (0,0,0,0)

		_PlayerPos ("pp", Vector) = (0,0,0,0)

		_LightCount ("Light Count", Int) = 0
		_DayLight ("Day Light Value", Float) = 1
		_DayLightColorNet ("Coloured Day Light Value", Float) = 1
		_SicknessIntensity ("Sickness Intensity", Float) = 0.5
		_SicknessSpeed ("Sickness Speed", Float) = 4
		_DayColor("Day Color Value", Color) = (1, 1, 1, 1)
		_RedFactor("Red Factor", Float) = 0

		_NightVisionStrength ("Night Vision Strength", Float) = 0

		[HDR]_AuraColor  ("AuraColor", Color) = (1, 1, 1, 1)
		_FishTankColor  ("Fish Tank Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
				float4 screenPos : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}

			sampler2D _MainTex;
			sampler2D _VisTex;
			sampler2D _FishTankVisTex;
			sampler2D _EntityTex;
			sampler2D _VisEntityTex;
			sampler2D _LightTex0;
			sampler2D _HighlightTex;
			sampler2D _BlurredHighlightTex;
			sampler2D _NoiseTex;
			float _DayLightColorNet;
			float _DayLight;
			float _SicknessIntensity;
			float _SicknessSpeed;
			float4 _AuraColor;
			float4 _FishTankColor;
			float4 _DayColor;
			float _RedFactor;
			float4 _LightPos0;
			float _NightVisionStrength;



			int _LightCount;

			float4 getNetLightColor(float2 uv)
			{

			    float4 netCol = tex2D(_LightTex0, uv);

			    return netCol;
			}

			float4 getPosLightColor(float2 uv)
			{

			    float4 netCol = tex2D(_LightTex0, uv);

			    return netCol;
			}

			float4 greyscale(float4 col)
			{
	            float grey = 0.3 * col.r + 0.59 * col.g + 0.11 * col.b;

	            return float4(grey, grey, grey, col.a);
			}

			float4 redden(float4 col)
			{

	           // float grey = 0.21 * col.r + 0.71 * col.g + 0.07 * col.b;
	            float grey = 0.3 * col.r + 0.59 * col.g + 0.11 * col.b;

	            float redFactor = 0.4 * (col.r - grey) / grey;

	            if (redFactor < 0.55 || col.r < 0.3) {
	                redFactor = 0;
	            }
	            else {
	                redFactor = 1;
	            }

	            redFactor = lerp(1, clamp(redFactor, 0, 1), _RedFactor);

	            return lerp(float4(grey, grey, grey, col.a), col, redFactor);
			}

			float3 getNoise(float2 uv)
            {
                float3 noise = tex2D(_NoiseTex, uv * 100 + _Time * 50);
                noise = mad(noise, 2.0, -0.5);

                return noise/255;
            }

			float4 frag (v2f i) : SV_Target
			{

			    float aspect = _ScreenParams.y / _ScreenParams.x;
				float2 p = i.screenPos.xy + getNoise(i.uv * float2(1, aspect)) * 0.3;


				p.y *= aspect;



				float2 rel = p - float2(0.5, 0.5 * aspect - 0.022);
				rel.y *= 1.2;

				float lLength = length(rel) ;


                if (_SicknessIntensity > 0) {
                    float2 cUv = (i.uv * 2 - float2(1, 1));
                    float s = sin(_Time * _SicknessSpeed + lLength * 5 * cUv.y) * 0.5;
                    float c = cos(_Time * _SicknessSpeed + lLength * 5 * cUv.x) * 0.5;

                    float sickWobble = sin(_Time * 500 + cUv.y * 80) * _SicknessIntensity * 0.006 * sqrt(lLength);

                    i.uv.x += (_SicknessIntensity * s * cUv.x * sqrt(lLength) + sickWobble) * lerp(1, 0, i.uv.x * i.uv.x) * lerp(0, 1, i.uv.x * i.uv.x);
                    i.uv.y += _SicknessIntensity * c * cUv.y * sqrt(lLength) * lerp(1, 0, i.uv.y * i.uv.y) * lerp(0, 1, i.uv.y * i.uv.y);
			    }

			    float4 plainEntityCol = tex2D(_EntityTex, i.uv);

				float4 visEntityCol = redden(tex2D(_VisEntityTex, i.uv));
				float4 visCol = tex2D(_VisTex, i.uv);
				float4 visFishTankCol = tex2D(_FishTankVisTex, i.uv);
				float4 plainBackCol = redden(tex2D(_MainTex, i.uv));
				float4 backCol = redden(plainBackCol);

				float visS = visCol.a;
				float visUnion = 1 - visS;


				float lLengthFactor = lLength * lLength * 10;

				   // Mathf.Lerp((pos - from).sqrMagnitude, 1, dayManager.LightLevel) * dayManager.LightLevel



				float dayRetract = lerp(1, 0, lerp(lLengthFactor, 0, clamp(_DayLight * _DayLight * 1.2 - _SicknessIntensity, 0, 1)));
				float4 netDayEffect = dayRetract * _DayColor;




				//_AuraColor.a = visUnion;


				if (visCol.a > 0) {
				    backCol = lerp(backCol, visEntityCol, visEntityCol.a);
				}


				float d = clamp(1 - lLength * lerp(4, 1, _NightVisionStrength), 0, 1);
				float4 auraColor = float4(d,d,d,1) * clamp(1 - _DayLightColorNet * 1, 0, 1);


				float effectStrength = 1.3 - greyscale(backCol).r * 1;

				float4 col1;
				if (plainEntityCol.a == 0) {
				    float4 lightCol = getNetLightColor(i.uv);
                   // netDayEffect = max(netDayEffect, auraColor * effectStrength);
				    float4 netLightCol = lerp(lightCol + netDayEffect, netDayEffect, clamp(netDayEffect, 0, 1) * 0.8);

				    //backCol = lerp(backCol, greyscale(plainBackCol), clamp(auraColor  - lightCol, 0, 1));

					col1 = max(backCol * netLightCol, greyscale(plainBackCol) * auraColor * effectStrength);
				}
				else {
				    float4 lightCol = getPosLightColor(i.uv);
				    plainEntityCol = lerp(plainBackCol, plainEntityCol, plainEntityCol.a);
				    float4 entityCol = redden(plainEntityCol);



				    if (greyscale(plainEntityCol).r > 0.85) {
				    //    netDayEffect = max(netDayEffect, auraColor);
				    }
				    else {
				        if (entityCol.a == 1) {
                   //         netDayEffect = max(netDayEffect, auraColor * effectStrength);
                            effectStrength = 0;
                        }
				    }




				    float4 netLightCol = lerp(lightCol + netDayEffect, netDayEffect, clamp(netDayEffect, 0, 1) * 0.8);

				   // entityCol = lerp(entityCol, greyscale(entityCol), clamp(auraColor - lightCol, 0, 1));
					//col1 = (entityCol * entityCol.a + backCol * (1 - entityCol.a)) * lightCol;

					col1 = max(entityCol * netLightCol, greyscale(plainEntityCol) * auraColor * effectStrength);
				}


				float4 col2 = 0.6 * backCol * netDayEffect;
				col2.a = 1;
				col1.a = 1;

				//col1 = lerp(col1, col1 + float4(0, 1, 0, 0), _SicknessIntensity * 0.25);
				col1 += clamp(_SicknessIntensity - 0.2, 0, 1) * float4(0, 0.5, 0, 0);

                float4 colOut = lerp(col2, col1, visUnion);

                float4 HighlightCol = tex2D(_HighlightTex, i.uv);
                float4 BlurredHighlightCol = tex2D(_BlurredHighlightTex, i.uv);

                colOut = (1.0 - visFishTankCol.a) * colOut + colOut * visFishTankCol.a * _FishTankColor;

				//return lerp(colOut, HighlightCol, HighlightCol.a);
				return colOut;


			}
			ENDCG
		}
	}
}
