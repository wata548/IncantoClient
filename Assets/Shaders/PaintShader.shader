Shader "Custom/PaintShader"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
        _MainTex ("Texture", 2D) = "black" {}
        _Pos ("Pos", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Radius", Float) = 0.1
    }
    SubShader
    {
        Pass
        {
            ZTest Always
            Cull Off
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appData
            {
                float2 uv:TEXCOORD0;
                float4 vertex:POSITION;
            };

            struct v2f
            {
                float2 uv:TEXCOORD0;
                float4 vertex:SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Pos;
            float _Radius;
            fixed4 _Color;

            v2f vert(appData i)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
                return o;
            }

            fixed4 frag(v2f i): SV_Target
            {
                fixed4 current = tex2D(_MainTex, i.uv);
                fixed4 color = length(i.uv - _Pos.xy) / _Radius * 2 > 1
                                   ? current
                                   : _Color;
                return color;
            }
            ENDCG

        }
    }
}