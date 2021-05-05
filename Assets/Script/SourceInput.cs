using UnityEngine;

public class SourceInput : MonoBehaviour
{
    [SerializeField] string _CameraSource;
    [SerializeField] Texture _TextureSource = null;
    [SerializeField] Vector2Int _Resolution = new Vector2Int(1080, 1080);
    [SerializeField] bool _HFlip = false;
    [SerializeField] bool _Square = true;

    WebCamTexture _Webcam;
    RenderTexture _Buffer;

    public Texture SourceTexture 
        => _TextureSource? (Texture)_TextureSource : (Texture)_Buffer;

    void Start()
    {
        if(_TextureSource) return;

        _Webcam = new WebCamTexture(_CameraSource, _Resolution.x, _Resolution.y, 30);
        _Buffer = new RenderTexture(_Resolution.x, _Resolution.y, 0);
        _Webcam.Play();
    }

    void OnDestroy()
    {
        if (_Webcam) Destroy(_Webcam);
        if (_Buffer) Destroy(_Buffer);
    }

    void Update()
    {
        if (_TextureSource) return;
        if (!_Webcam.didUpdateThisFrame) return;

        var vflip = _Webcam.videoVerticallyMirrored;
        var scale = Vector2.zero;
        var offset = Vector2.zero;

        if(_Square)
        {
            var camAspect = (float)_Webcam.width / _Webcam.height;
            var texAaspect = (float)_Resolution.x / _Resolution.y;
            var gap = texAaspect / camAspect;
            scale = new Vector2(gap * (_HFlip ? -1 : 1), vflip ? -1 : 1);
            offset = new Vector2(_HFlip ? .5f + gap / 2 : (1 - gap) / 2, vflip ? 1 : 0);
        }
        else
        {
            scale = new Vector2(_HFlip ? -1 : 1, vflip ? -1 : 1);
            offset = new Vector2(_HFlip ? 1 : 0 / 2, vflip ? 1 : 0);
        }

        Graphics.Blit(_Webcam, _Buffer, scale, offset);
    }

}
