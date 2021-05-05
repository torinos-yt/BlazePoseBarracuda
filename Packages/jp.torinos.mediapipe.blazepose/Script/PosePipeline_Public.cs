using UnityEngine;

namespace MediaPipe.BlazePose {

//
// Public part of the pose pipeline class
//

partial class PosePipeline
{
    const int FullKeyPointCount = 33;
    const int UpperKeyPointCount = 25;
    public int KeyPointCount
        => _upperbody ? UpperKeyPointCount : FullKeyPointCount;

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

    public Vector3 GetKeyPoint(KeyPoint point)
      => ReadCache[(int)point];

    public Vector3 GetKeyPoint(int index)
      => ReadCache[index];

    public ComputeBuffer InputBuffer
      => _buffer.input;

    public ComputeBuffer KeyPointBuffer
      => _buffer.filter;

    public ComputeBuffer PoseRegionBuffer
      => _buffer.region;

    public ComputeBuffer PoseRegionCropBuffer
      => _buffer.crop;

    public RenderTexture SegmentationBuffer
      => _detector.landmark.SegmentationBuffer;

    public ComputeBuffer PoseBuffer
      => _detector.pose.DetectionBuffer;

    public bool UseAsyncReadback { get; set; } = true;

    public PosePipeline(ResourceSet resources, bool upperbody)
    {
      _resources = resources;
      _upperbody = upperbody;
      ReadbackBytes = KeyPointCount * sizeof(float) * 4;

      AllocateObjects();
    }

    public void Dispose()
      => DeallocateObjects();

    public void ProcessImage(Texture image)
      => RunPipeline(image);

}

} // namespace MediaPipe.BlazePose
