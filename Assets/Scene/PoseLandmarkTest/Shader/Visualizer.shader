Shader "Hidden/MediaPipe/PoseLandmark/Visualizer"
{
    CGINCLUDE

    #include "UnityCG.cginc"

    StructuredBuffer<float4> _KeyPoints;
    
    struct v2f
    {
        float4 pos : SV_Position;
        float score : SCORE;
    };

    v2f VertexKey(uint vid : SV_VertexID)
    {
        // Key point
        float4 key = _KeyPoints[(vid / 4)+1];
        float2 p = key.xy;
        p.y = 1 - p.y;
        float score = key.w;

        // Marker shape (+)
        const float size = 0.015;
        uint vtid = vid & 3;
        p.x += size * lerp(-1, 1, vtid > 0) * (vtid < 2);
        p.y += size * lerp(-1, 1, vtid > 2) * (vtid > 1);

        // Clip space to screen space
        p = p * 2 - 1;
        p.x *= _ScreenParams.y / _ScreenParams.x;

        v2f o;
        o.pos = float4(p, 1, 1);
        o.score = score;

        return o;
    }

    float4 FragmentKey(v2f i) : SV_Target
    {
        return float4(1, 1, 1, i.score);
    }

    ENDCG

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex VertexKey
            #pragma fragment FragmentKey
            ENDCG
        }
    }
}
