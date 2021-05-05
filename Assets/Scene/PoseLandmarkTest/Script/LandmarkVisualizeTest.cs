using UnityEngine;
using UnityEngine.UI;
using MediaPipe.PoseLandmark;

public class LandmarkVisualizeTest : MonoBehaviour
{
    [SerializeField] SourceInput _Input;
    [SerializeField] RawImage _Image;
    [Space]
    [SerializeField] ResourceSet _Resources;
    [SerializeField] Shader _Shader;
    [Space]
    [SerializeField] bool _UpperBodyOnly = true;
    [SerializeField] bool _DisplaySegmentation = true;

    PoseLandmarkDetector _Detector;
    Material _Material;

    ComputeBuffer _KeyPointArgs;

    void Start()
    {
        _Detector = new PoseLandmarkDetector(_Resources, _UpperBodyOnly);
        _Material = new Material(_Shader);

        var cbType = ComputeBufferType.IndirectArguments;
        _KeyPointArgs = new ComputeBuffer(4, sizeof(uint), cbType);
        _KeyPointArgs.SetData(new [] {_Detector.VertexCount * 4, 1, 0, 0});
    }

    void OnDestroy()
    {
        _Detector.Dispose();
        Destroy(_Material);

        _KeyPointArgs.Dispose();
    }

    void LateUpdate()
    {
        _Detector.ProcessImage(_Input.SourceTexture);
        _Image.texture = 
            _DisplaySegmentation ? _Detector.SegmentationBuffer : _Input.SourceTexture;
    }

    void OnRenderObject()
    {
        _Material.SetBuffer("_KeyPoints", _Detector.OutputBuffer);
        _Material.SetPass(0);
        Graphics.DrawProceduralIndirectNow(MeshTopology.Lines, _KeyPointArgs, 0);
    }
}
