Shader "Custom/BossWall" {
	Properties {
		_Texture("Texture", 2D) = "white" {}
		_Height ("Height", Range(0.0, 1.0)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard 
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		sampler2D _Texture;
		float _Height;
		float4 _Texture_ST;
		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed2 uv = IN.uv_MainTex;
			uv = uv * _Texture_ST;
			// uv.x *= 15;
			// uv.y *= 15;
			
			fixed4 tex = tex2D (_Texture, uv);
			float light = 1 - IN.worldPos.y * _Height;
			o.Albedo = fixed4(tex.r * light, tex.g * light, tex.b * light, 1);
		}
		ENDCG
	}
	FallBack "Diffuse"
}