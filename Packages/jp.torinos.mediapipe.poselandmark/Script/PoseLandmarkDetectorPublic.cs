using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace MediaPipe.PoseLandmark {

public sealed partial class PoseLandmarkDetector
{
    const int FullVertexCount = 33;
    const int UpperVertexCount = 25;
    public int VertexCount
        => _upperbody ? UpperVertexCount : FullVertexCount;

    public const int ImageSize = 256;

    // index == 0 : float4(pose_score, 0, 0, 0)
    // index >= 1 : float4(key.x, key.y, key.z, key_score)
    public ComputeBuffer OutputBuffer
        => _postBuffer;

    public IEnumerable<Vector4> VertexArray
        => PostReadCache.Skip(1);

    public RenderTexture SegmentationBuffer
        => _segmentationBuffer;

    public PoseLandmarkDetector(ResourceSet resources, bool upperbody)
    {
        _resources = resources;
        _upperbody = upperbody;
        AllocateObjects();
    }

    public void Dispose()
        => DeallocateObjects();

    public void ProcessImage(Texture image)
      => RunModel(Preprocess(image));

    public void ProcessImage(ComputeBuffer buffer)
      => RunModel(buffer);

    public enum KeyPoint
    {
        Nose,
        LeftEye1, LeftEye2, LeftEye3,
        RightEye1, RightEye2, RightEye3,
        LeftEar, RightEar,
        MouseLeft, MouseRight,
        LeftShoulder, RightShoulder,
        LeftElbow, RightElbow,
        LeftWrist, RightWrist,
        LeftPinky, RightPinky,
        LeftIndex, RightIndex,
        LeftThumb, RightTumb,
        LeftHip, RightHip,
        LeftKnee, RightKnee,
        LeftAnkle, RightAnkle,
        LeftHeel, RightHeel,
        LeftFootIndex, RightFootIndex
    }

    public Vector2 GetKeyPoint(KeyPoint point)
        => PostReadCache[(int)point + 1];

    public float Score
        => PostReadCache[0].x;

    public float Handedness
        => PostReadCache[0].y;
}

}
