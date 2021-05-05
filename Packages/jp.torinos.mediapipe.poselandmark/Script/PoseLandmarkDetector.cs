using Unity.Barracuda;
using UnityEngine;

namespace MediaPipe.PoseLandmark {

public sealed partial class PoseLandmarkDetector : System.IDisposable
{
    ResourceSet _resources;
    ComputeBuffer _preBuffer;
    ComputeBuffer _postBuffer;
    RenderTexture _segmentationBuffer;
    IWorker _worker;
    bool _upperbody;

    void AllocateObjects()
    {
        var modelResource = _upperbody ? _resources.model_upperbody : _resources.model_fullbody;
        var model = ModelLoader.Load(modelResource);
        if(!_upperbody) _resources.postprocess.EnableKeyword("FULL_BODY");
        else            _resources.postprocess.DisableKeyword("FULL_BODY");
        _preBuffer = new ComputeBuffer(ImageSize * ImageSize * 3, sizeof(float));
        _postBuffer = new ComputeBuffer(VertexCount + 1, sizeof(float) * 4);
        _postReadCache = new Vector4[VertexCount + 1];
        _segmentationBuffer = new RenderTexture(128, 128, 0, RenderTextureFormat.RFloat);
        _worker = model.CreateWorker();
    }

    void DeallocateObjects()
    {
        _preBuffer?.Dispose();
        _preBuffer = null;

        _postBuffer?.Dispose();
        _postBuffer = null;

        _segmentationBuffer?.Release();
        _segmentationBuffer = null;

        _worker?.Dispose();
        _worker = null;
    }

    ComputeBuffer Preprocess(Texture source)
    {
        var pre = _resources.preprocess;
        pre.SetTexture(0, "_Texture", source);
        pre.SetBuffer(0, "_Tensor", _preBuffer);
        pre.Dispatch(0, ImageSize / 8, ImageSize / 8, 1);
        return _preBuffer;
    }

    void RunModel(ComputeBuffer input)
    {
        // Run the BlazeFace model.
        using (var tensor = new Tensor(1, ImageSize, ImageSize, 3, input))
            _worker.Execute(tensor);

        // Postprocessing
        var post = _resources.postprocess;
        post.SetBuffer(0, "_Landmark", _worker.PeekOutputBuffer("ld_3d"));
        post.SetBuffer(0, "_Flag", _worker.PeekOutputBuffer("output_poseflag"));
        post.SetBuffer(0, "_Output", _postBuffer);
        post.Dispatch(0, 1, 1, 1);

        var segRT = _worker.CopyOutputToTempRT("output_segmentation", 128, 128);
        Graphics.Blit(segRT, _segmentationBuffer);
        RenderTexture.ReleaseTemporary(segRT);

        // Read cache invalidation
        _postRead = false;
    }

    Vector4[] _postReadCache;
    bool _postRead;

    Vector4[] PostReadCache
      => _postRead ? _postReadCache : UpdatePostReadCache();

    Vector4[] UpdatePostReadCache()
    {
        _postBuffer.GetData(_postReadCache, 0, 0, VertexCount + 1);
        _postRead = true;
        return _postReadCache;
    }
}

}
