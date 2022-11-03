Shader "Graph/Point Surface"
{
	Properties{
	_Smoothness("Smoothness",Range(0,1)) = 0.5
	}

	SubShader
	{
		CGPROGRAM
		#pragma surface ConfigureSurface Standard fullforwardshadows
		#pragma target 3.0

		struct Input
		{
			float3 worldPos;
		};

		float _Smoothness;

		void ConfigureSurface (Input input,inout SurfaceOutputStandard surface)//inout关键字 surface传入其中使用 并且作为最终surface传出
		{
			surface.Albedo = input.worldPos * .5 + .5;
			surface.Smoothness = _Smoothness;
		}

		ENDCG
	}

	FallBack "Diffuse"
}
