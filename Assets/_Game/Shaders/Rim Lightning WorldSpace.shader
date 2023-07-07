Shader "Toony Colors Free/Rim Lighting - WorldSpace"
{
	Properties
	{
		//TOONY COLORS
		_Color("Color", Color) = (0.5,0.5,0.5,1.0)
		_HColor("Highlight Color", Color) = (0.6,0.6,0.6,1.0)
		_SColor("Shadow Color", Color) = (0.3,0.3,0.3,1.0)

		//DIFFUSE
		_MainTex("Main Texture (RGB)", 2D) = "white" {}

	//TOONY COLORS RAMP
	_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}

	//RIM LIGHT
	_RimColor("Rim Color", Color) = (0.8,0.8,0.8,0.6)
	_RimMin("Rim Min", Range(0,1)) = 0.5
	_RimMax("Rim Max", Range(0,1)) = 1.0

	 _Scale("WORLD - SPACE Texture Scale", Float) = 1.0
	}

		SubShader
	{
		Tags { "RenderType" = "Opaque" }

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom
		#pragma target 2.0
		#pragma glsl


		//================================================================
		// VARIABLES

		fixed4 _Color;
		sampler2D _MainTex;

		fixed4 _RimColor;
		fixed _RimMin;
		fixed _RimMax;
		float4 _RimDir;

		float _Scale;

		struct Input
		{
			half2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
			float3 worldPos;
		};

		//================================================================
		// CUSTOM LIGHTING

		//Lighting-related variables
		fixed4 _HColor;
		fixed4 _SColor;
		sampler2D _Ramp;

		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Alpha;
		};

		inline half4 LightingToonyColorsCustom(SurfaceOutputCustom s, half3 lightDir, half3 viewDir, half atten)
		{
			s.Normal = normalize(s.Normal);
			fixed ndl = max(0, dot(s.Normal, lightDir) * 0.5 + 0.5);

			fixed3 ramp = tex2D(_Ramp, fixed2(ndl,ndl));
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
			_SColor = lerp(_HColor, _SColor, _SColor.a);	//Shadows intensity through alpha
			ramp = lerp(_SColor.rgb,_HColor.rgb,ramp);
			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp;
			c.a = s.Alpha;
		#if (POINT || SPOT)
			c.rgb *= atten;
		#endif
			return c;
		}


		//================================================================
		// SURFACE FUNCTION

		void surf(Input IN, inout SurfaceOutputCustom o)
		{
			//fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);



			float2 UV;
			fixed4 c;

			if (abs(IN.worldNormal.x) > 0.5)
			{
				UV = IN.worldPos.yz + float2(0, 0.5); // side
				c = tex2D(_MainTex, UV * _Scale); // use WALLSIDE texture
				c = c + fixed4(-.2, -.2, -.2, 0); // littlebit different
			}
			else if (abs(IN.worldNormal.z) > 0.5)
			{
				UV = IN.worldPos.xy + float2(0.5, 0); // front
				c = tex2D(_MainTex, UV * _Scale); // use WALL texture
				c = c + fixed4(-.2, -.2, -.2, 0); // littlebit different
			}
			else
			{
				UV = IN.worldPos.xz + float2(0.5, 0.5); // top
				c = tex2D(_MainTex, UV * _Scale); // use FLR texture
				c = c + fixed4(-.05, -.05, -.05, 0); // littlebit different than playerColor
			}

			o.Albedo = c.rgb * _Color.rgb;
			o.Alpha = _Color.a;
			//o.Alpha = mainTex.a * _Color.a;
			//o.Alpha = mainTex.a * _Color.a;

			//Rim
			float3 viewDir = normalize(IN.viewDir);
			half rim = 1.0f - saturate(dot(viewDir, o.Normal));
			rim = smoothstep(_RimMin, _RimMax, rim);
			o.Emission += (_RimColor.rgb * rim) * _RimColor.a;
		}

		ENDCG
	}

		Fallback "Diffuse"
			CustomEditor "TCF_MaterialInspector"
}
