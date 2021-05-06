using UnityEngine;
using Unity.Barracuda;
using MediaPipe;

namespace MediaPipe.BlazePose {

[CreateAssetMenu(fileName = "BlazePose",
                 menuName = "ScriptableObjects/MediaPipe/BlazePose Resource Set")]
public sealed class ResourceSet : ScriptableObject
{
    public PoseDetect.ResourceSet pose_resource;
    public PoseLandmark.ResourceSet landmark_resource;
    public ComputeShader pipeline;
    public ComputeShader postprocess;
}

}
