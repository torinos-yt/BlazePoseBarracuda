using UnityEngine;
using UnityEngine.UI;
using MediaPipe.PoseDetect;

public class VisualizeTest : MonoBehaviour
{
    [SerializeField] SourceInput _Input;
    [SerializeField] RawImage _Image;
    [Space]
    [SerializeField] ResourceSet _Resources;
    [SerializeField] Shader _Shader;
    [Space]
    [SerializeField] bool _UpperBody = true;

    PoseDetector _Detector;
    Material _Material;

    ComputeBuffer _BoxDrawArgs;
    ComputeBuffer _KeyPointArgs;

    void Start()
    {
        _Detector = new PoseDetector(_Resources);
        _Material = new Material(_Shader);

        var cbType = ComputeBufferType.IndirectArguments;
        _BoxDrawArgs = new ComputeBuffer(4, sizeof(uint), cbType);
        _KeyPointArgs = new ComputeBuffer(4, sizeof(uint), cbType);
        _BoxDrawArgs.SetData(new [] {6, 0, 0, 0});
        _KeyPointArgs.SetData(new [] {10, 0, 0, 0});
    }

    void OnDestroy()
    {
        _Detector.Dispose();
        Destroy(_Material);

        _BoxDrawArgs.Dispose();
        _KeyPointArgs.Dispose();
    }

    void LateUpdate()
    {
        _Detector.ProcessImage(_Input.SourceTexture);
        _Image.texture = _Input.SourceTexture;
    }

    void OnRenderObject()
    {
        _Material.SetBuffer("_Detections", _Detector.DetectionBuffer);
        _Material.SetInt("_UpperBody", _UpperBody?1:0);

        _Detector.SetIndirectDrawCount(_BoxDrawArgs);
        _Detector.SetIndirectDrawCount(_KeyPointArgs);

        // Bounding box
        _Material.SetPass(0);
        Graphics.DrawProceduralIndirectNow(MeshTopology.Triangles, _BoxDrawArgs, 0);

        // Full body bounding box
        _Material.SetPass(1);
        Graphics.DrawProceduralIndirectNow(MeshTopology.Lines, _KeyPointArgs, 0);
    }
}
