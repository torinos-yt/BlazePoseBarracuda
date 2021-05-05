#ifndef _BLAZEPOSEBARRACUDA_POSEREGION_HLSL_
#define _BLAZEPOSEBARRACUDA_POSEREGION_HLSL_

//
// Pose region tracking structure
//
// size = 24 * 4 byte
//
struct PoseRegion
{
    float4 box; // center_x, center_y, size, angle
    float4 dBox;
    float4x4 cropMatrix;
};

#endif
