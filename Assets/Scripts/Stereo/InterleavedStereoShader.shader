
Shader "Hidden/InterleavedStereoShader"
{
    Properties
    {
        _LeftEye  ("Left Eye (RGB)", Rect) = "white" {}
        _RightEye ("Right Eye (RGB)", Rect) = "white" {}
    }

    SubShader
    {
        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off
            Fog
            {
                Mode off
            }

            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma exclude_renderers d3d11 xbox360
            #pragma fragment frag
            #pragma vertex vert

            uniform sampler2D _LeftEye;
            uniform sampler2D _RightEye;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv  = v.texcoord;

                return o;
            }

            fixed4 frag(v2f i) : COLOR0
            {
                float3 left  = tex2D(_LeftEye, i.uv).rgb;
                float3 right = tex2D(_RightEye, i.uv).rgb;

                int x = fmod(i.uv.x * 1920, 2.0);
                return float4(lerp(left, right, x), 1.0);
            }
            ENDCG
        }
    }
}
