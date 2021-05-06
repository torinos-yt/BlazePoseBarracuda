using UnityEngine;
using Unity.Barracuda;

namespace MediaPipe.PoseDetect {

[CreateAssetMenu(fileName = "PoseDetector",
                 menuName = "ScriptableObjects/MediaPipe/PoseDetector Resource Set")]
public sealed class ResourceSet : ScriptableObject
{
    public NNModel model;
    public ComputeShader preprocess;
    public ComputeShader postprocess1;
    public ComputeShader postprocess2;
}

}
