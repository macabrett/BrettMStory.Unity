// This file must be copied into your Unity project somewhere
Shader "Hidden/DropShadow" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader {
		// No culling or depth
		Cull Off 
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Tags { "RenderType" = "Transparent"}

		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"
	
			sampler2D _MainTex;

			half4 frag(v2f_img i) : SV_Target	{
				float4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				return float4(0.0, 0.0, 0.0, col.a * 0.5);
			}
			ENDCG
		}
	}
}
