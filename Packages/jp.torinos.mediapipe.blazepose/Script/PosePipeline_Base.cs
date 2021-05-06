using MediaPipe.PoseDetect;
using MediaPipe.PoseLandmark;
using UnityEngine;

namespace MediaPipe.BlazePose {

//
// Basic implementation of the pose pipeline class
//

public sealed partial class PosePipeline : System.IDisposable
{
    const int CropSize = PoseLandmarkDetector.ImageSize;

    int InputWidth => _detector.pose.ImageSize;

    ResourceSet _resources;

    (PoseDetector pose, PoseLandmarkDetector landmark) _detector;

    (ComputeBuffer input, ComputeBuffer crop,
     ComputeBuffer region, ComputeBuffer filter) _buffer;

     bool _upperbody;

    void AllocateObjects()
    {
        _detector = (new PoseDetector(_resources.pose_resource),
                     new PoseLandmarkDetector(_resources.landmark_resource, _upperbody));

        if(!_upperbody) _resources.postprocess.EnableKeyword("FULL_BODY");
        else            _resources.postprocess.DisableKeyword("FULL_BODY");

        var inputBufferLength = 3 * InputWidth * InputWidth;
        var cropBufferLength = 3 * CropSize * CropSize;
        var regionStructSize = sizeof(float) * 24;
        var filterBufferLength = _detector.landmark.VertexCount * 2;

        _buffer = (new ComputeBuffer(inputBufferLength, sizeof(float)),
                   new ComputeBuffer(cropBufferLength, sizeof(float)),
                   new ComputeBuffer(1, regionStructSize),
                   new ComputeBuffer(filterBufferLength, sizeof(float) * 4));

        _readCache = new Vector4[KeyPointCount];
    }

    void DeallocateObjects()
    {
        _detector.pose.Dispose();
        _detector.landmark.Dispose();
        _buffer.input.Dispose();
        _buffer.crop.Dispose();
        _buffer.region.Dispose();
        _buffer.filter.Dispose();
    }
}

} // namespace MediaPipe.BlazePose
