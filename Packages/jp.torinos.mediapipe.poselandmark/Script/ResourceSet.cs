using UnityEngine;
using Unity.Barracuda;

namespace MediaPipe.PoseLandmark {

[CreateAssetMenu(fileName = "PoseLandmark",
                 menuName = "ScriptableObjects/MediaPipe/PoseLandmark Resource Set")]
public sealed class ResourceSet : ScriptableObject
{
    public NNModel model_fullbody;
    public NNModel model_upperbody;
    public ComputeShader preprocess;
    public ComputeShader postprocess;
}

}
