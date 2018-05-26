 Shader "Custom/Breakables" 
 {
     Properties 
     {
         _MainTex ("Base (RGB)", 2D) = "white" {}
		 _Color ("Tint", Color) = (1,1,1,1)
         _Crack1 ("Crack 1", 2D) = "white" {}
         _Crack2 ("Crack 2", 2D) = "white" {}
         _Crack3 ("Crack 3", 2D) = "white" {}
         [PerRendererData] _CrackIndex ("Index", Float ) = 1

		 _TransparentColor ("Transparent Color", Color) = (1,1,1,1)
		 _Threshold ("Threshhold", Float) = 0.1

		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
     }

     SubShader {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass 
		{
        CGPROGRAM
			#pragma vertex vert alpha:fade
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex    : SV_POSITION;
				fixed4 color     : COLOR;
				float2 texcoord  : TEXCOORD0;
				float2 uv_Crack1 : TEXCOORD1;
				float2 uv_Crack2 : TEXCOORD2;
			};
			
			fixed4 _Color;

			float _CrackIndex;
			fixed4 _TransparentColor;
			half _Threshold;

			float4  _Crack1_ST;
			float4  _Crack2_ST; 

			sampler2D _MainTex;
			sampler2D _Crack1;
			sampler2D _Crack2;
			sampler2D _Crack3;
	
			v2f vert(appdata_t IN, float2 uv_Crack1: TEXCOORD1, float2 uv_Crack2: TEXCOORD2)
			{
				v2f OUT;

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				OUT.uv_Crack1 = uv_Crack1;
				OUT.uv_Crack2 = uv_Crack2;

				return OUT;
			}

			fixed4 SampleSpriteTexture(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
                half4 cracks;

				if( _CrackIndex <= 1.0f ) cracks = (1).rrrr;
             	else if( _CrackIndex <= 2.0f ) cracks = tex2D(_Crack1, IN.uv_Crack1 * _Crack1_ST);
             	else if( _CrackIndex <= 3.0f ) cracks = tex2D(_Crack2, IN.uv_Crack2 * _Crack2_ST);
             	else cracks = tex2D (_Crack3, IN.texcoord);

             	half3 transparent_diff = cracks.xyz - _TransparentColor.xyz;
             	half transparent_diff_squared = dot(transparent_diff, transparent_diff);

				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

				c.rgb = c.rgb;
				c.a = c.a * (cracks.a * transparent_diff_squared);

				return c; 
			}
         ENDCG
		}
     }

     FallBack "Diffuse"
 }

