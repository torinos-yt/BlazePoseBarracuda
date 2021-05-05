using UnityEngine;
using UnityEngine.UI;

namespace MediaPipe.BlazePose {

public sealed class BlazePoseVisualizeTest : MonoBehaviour
{
    [SerializeField] SourceInput _webcam;
    [Space]
    [SerializeField] ResourceSet _resources;
    [SerializeField] Shader _keyPointShader;
    [SerializeField] Shader _poseRegionShader;
    [Space]
    [SerializeField] RawImage _mainUI;
    [SerializeField] RawImage _cropUI;
    [Space]
    [SerializeField] bool _upperBodyOnly = true;

    PosePipeline _pipeline;
    (Material keys, Material region) _material;


    void Start()
    {
        _pipeline = new PosePipeline(_resources, _upperBodyOnly);
        _material = (new Material(_keyPointShader),
                     new Material(_poseRegionShader));

        // Material initial setup
        _material.keys.SetBuffer("_KeyPoints", _pipeline.KeyPointBuffer);
        _material.region.SetBuffer("_Image", _pipeline.PoseRegionCropBuffer);

        // UI setup
        _cropUI.material = _material.region;
    }

    void OnDestroy()
    {
        _pipeline.Dispose();
        Destroy(_material.keys);
        Destroy(_material.region);
    }

    void LateUpdate()
    {
        // Feed the input image to the Pose pipeline.
        _pipeline.ProcessImage(_webcam.SourceTexture);

        // UI update
        _mainUI.texture = _webcam.SourceTexture;
        _cropUI.texture = _webcam.SourceTexture;
    }

    void OnRenderObject()
    {
        // Key point circles
        _material.keys.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 96, _pipeline.KeyPointCount);

        // Skeleton lines
        // _material.keys.SetPass(1);
        // Graphics.DrawProceduralNow(MeshTopology.Lines, 2, 4 * 5 + 1);
    }
}

} // namespace MediaPipe.BlazePose
