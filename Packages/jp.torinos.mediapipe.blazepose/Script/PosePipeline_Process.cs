using UnityEngine;

namespace MediaPipe.BlazePose {

//
// Image processing part of the pose pipeline class
//

partial class PosePipeline
{
    void RunPipeline(Texture input)
    {
        var cs_pipeline = _resources.pipeline;
        var cs_poseprocess = _resources.postprocess;

        // Letterboxing scale factor
        var scale = new Vector2
          (Mathf.Max((float)input.height / input.width, 1),
           Mathf.Max(1, (float)input.width / input.height));

        // Image scaling and padding
        cs_pipeline.SetInt("_spad_width", InputWidth);
        cs_pipeline.SetVector("_spad_scale", scale);
        cs_pipeline.SetTexture(0, "_spad_input", input);
        cs_pipeline.SetBuffer(0, "_spad_output", _buffer.input);
        cs_pipeline.Dispatch(0, InputWidth / 8, InputWidth / 8, 1);

        // pose detection
        _detector.pose.ProcessImage(_buffer.input);

        // Hand region bounding box update
        cs_pipeline.SetFloat("_bbox_dt", Time.deltaTime);
        cs_pipeline.SetInt("_UpperBody", _upperbody ? 1 : 0);
        cs_pipeline.SetBuffer(1, "_bbox_count", _detector.pose.CountBuffer);
        cs_pipeline.SetBuffer(1, "_bbox_pose", _detector.pose.DetectionBuffer);
        cs_pipeline.SetBuffer(1, "_bbox_region", _buffer.region);
        cs_pipeline.Dispatch(1, 1, 1, 1);

        // Hand region cropping
        cs_pipeline.SetTexture(2, "_crop_input", input);
        cs_pipeline.SetBuffer(2, "_crop_region", _buffer.region);
        cs_pipeline.SetBuffer(2, "_crop_output", _buffer.crop);
        cs_pipeline.Dispatch(2, CropSize / 8, CropSize / 8, 1);

        // Hand landmark detection
        _detector.landmark.ProcessImage(_buffer.crop);

        // Key point postprocess
        cs_poseprocess.SetFloat("_post_dt", Time.deltaTime);
        cs_poseprocess.SetFloat("_post_scale", scale.y);
        cs_poseprocess.SetBuffer(0, "_post_input", _detector.landmark.OutputBuffer);
        cs_poseprocess.SetBuffer(0, "_post_region", _buffer.region);
        cs_poseprocess.SetBuffer(0, "_post_output", _buffer.filter);
        cs_poseprocess.Dispatch(0, 1, 1, 1);

        // Read cache invalidation
        InvalidateReadCache();
    }
}

} // namespace MediaPipe.BlazePose
