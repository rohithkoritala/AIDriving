// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "Hidden/SegmentationShader" {

	Properties {
		_MainTex ("", 2D) = "white" {}
		_CutOff ("", Float) = 0.3
		_ObjectColor("", Color) = (0, 0, 0, 1)
	}

	CGINCLUDE

	#include "UnityCG.cginc"

	uniform fixed4 _ObjectColor;
	uniform sampler2D _MainTex;
	sampler2D _CameraDepthNormalsTexture;
	uniform float4 _MainTex_ST;
	uniform fixed _Cutoff;

	struct input{
		float4 vertex : POSITION;
		float4 texcoord : TEXCOORD0;
	};

	struct v2f {
	    float4 pos : SV_POSITION;
	    float2 uv : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
	};

	v2f vert( input v ) {
	    v2f o;
	    o.pos = UnityObjectToClipPos(v.vertex);
		o.uv2 = v.texcoord;
	    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
	    return o;
	}

	fixed4 frag(v2f i) : SV_Target {
		/*fixed4 col = tex2D(_MainTex, i.uv2);
                
        float4 NormalAndDepth;
        DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv2), NormalAndDepth.w, NormalAndDepth.xyz);
                
        col.rgb = 1 - NormalAndDepth.w;
        return col;*/
		
		fixed4 tex = tex2D( _MainTex, i.uv);
		if(tex.a < _Cutoff)
			discard;
		//clip( tex.a - _Cutoff );

		return _ObjectColor;
	}

	ENDCG

	SubShader{
		Tags { }
		Pass {

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag

			ENDCG
		}
	}

	FallBack "Hidden/InternalErrorShader"
}