#ifndef _POSEDETECTBARRACUDA_STRUCT_H_
#define _POSEDETECTBARRACUDA_STRUCT_H_

// Detection structure: The layout of this structure must be matched with the
// one defined in Detection.cs
struct PoseDetection
{
    float2 center;
    float2 extent;
    float2 keyPoints[4];
    float score;
    // Padding for StructuredBuffer performance.
    // Details : https://developer.nvidia.com/content/understanding-structured-buffer-performance
    float3 pad;
};

#endif
