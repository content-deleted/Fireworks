// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/LineBend"
{
    Properties
    {
        _BendLocation ("Hold Point", Vector) = (0,0,0)
        _BendDir ("Bend Direction", Vector) = (0,0,0)
        _Start ("Start", Vector) = (0,0,0)
        _End ("End", Vector) = (0,0,0)
    }
    SubShader
    {
        // Might change this later for transparent
        Cull Off ZWrite On ZTest Always  

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
            };

            uniform float3 _BendLocation;
            uniform float3 _BendDir;
            uniform float3 _Start;
            uniform float3 _End;

            v2f vert (appdata v)
            {
                v2f o;
                float4 worldPos = mul (unity_ObjectToWorld, v.vertex.xyz);
                o.vertex = worldPos;
                // Controls the amount the line bends overall
                o.vertex.xyz += (distance(worldPos.xyz, _Start) * distance(worldPos.xyz, _End)) * _BendDir / 20;
                // Controls the offset twards the end where the point is located
                o.vertex.xyz += (distance(worldPos.xyz, _Start) * distance(worldPos.xyz, _End)) * normalize( (_BendLocation + _BendDir) - worldPos.xyz) / 15;
                o.vertex = UnityWorldToClipPos(o.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                // caluclate color offset for channels not blue
                float b = cos(_Time*200) * 0.1 + 0.8;
                // alpha offset
                float a = 1 - cos(_Time) * abs(i.uv.x - 0.5)/4;
                float4 rgb = float4(b,b,1,a);
                return rgb;
            }
            ENDCG
        }
    }
}
