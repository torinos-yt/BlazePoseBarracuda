Shader "Hidden/MediaPipe/PoseDetector/Visualizer"
{
    CGINCLUDE

    #include "UnityCG.cginc"
    #include "Packages/jp.torinos.mediapipe.posedetector/Shader/Struct.hlsl"

    #define PI 3.14159265359

    StructuredBuffer<PoseDetection> _Detections;
    int _UpperBody;

    float2x2 rot2D(float angle)
    {
        return float2x2(cos(angle), -sin(angle),
                        sin(angle),  cos(angle));
    }

    float4 VertexBox(uint vid : SV_VertexID,
                     uint iid : SV_InstanceID) : SV_Position
    {
        PoseDetection d = _Detections[iid];

        // Rotation
        float2 hip = d.keyPoints[0];
        float2 shoulder = d.keyPoints[2];

        float target = PI * .5;
        float angle = target - atan2(-(shoulder.y - hip.y), shoulder.x - hip.x);
        angle = angle - 2 * PI * floor((angle + PI) / (2 * PI));

        // Bounding box
        float2 center = _UpperBody ? shoulder : hip;
        float2 roi = _UpperBody ? d.keyPoints[3] :d.keyPoints[1];
        float size = sqrt((roi.x - center.x) * (roi.x - center.x) +
                          (roi.y - center.y) * (roi.y - center.y)) * 2.0 * 1.5;
        float x = size * lerp(-.5, .5, vid & 1);
        float y = size * lerp(-.5, .5, vid < 2 || vid == 5);

        float2 vert = mul(rot2D(angle), float2(x, y));
        x = vert.x + center.x;
        y = vert.y + center.y;

        // Clip space to screen space
        x = (2 * x - 1) * _ScreenParams.y / _ScreenParams.x;
        y = (2 * y - 1);

        return float4(x, y, 1, 1);
    }

    float4 FragmentBox(float4 position : SV_Position) : SV_Target
    {
        return float4(1, 0, 0, 0.5);
    }

    float4 VertexKey(uint vid : SV_VertexID,
                     uint iid : SV_InstanceID) : SV_Position
    {
        PoseDetection d = _Detections[iid];

        // Key point
        float2 p = d.keyPoints[(vid / 4) * 2];

        // Marker shape (+)
        const float size = 0.01;
        uint vtid = vid & 3;
        p.x += size * lerp(-1, 1, vtid > 0) * (vtid < 2);
        p.y += size * lerp(-1, 1, vtid > 2) * (vtid > 1);

        // Line shoulder to hip
        p = vid > 7 ? d.keyPoints[(vid%2) * 2] : p;

        // Clip space to screen space
        p = p * 2 - 1;
        p.x *= _ScreenParams.y / _ScreenParams.x;

        return float4(p, 1, 1);
    }

    float4 FragmentKey(float4 position : SV_Position) : SV_Target
    {
        return float4(0, 0, 1, 0.9);
    }

    ENDCG

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex VertexBox
            #pragma fragment FragmentBox
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex VertexKey
            #pragma fragment FragmentKey
            ENDCG
        }
    }
}
