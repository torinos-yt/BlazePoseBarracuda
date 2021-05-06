BlazePoseBarracuda  
=========================

![img](https://i.imgur.com/wTY0Llx.gif)  

Thisi is Pose landmark tracker that works with a monocular color camera.  
This works by using the [MediaPipe Pose] model in [Unity Barracuda].

BlazePoseBaracuda is based on [HandPoseBarracuda] implementation by [keijiro].  
In his repositories, There are several tracking solutions using Barracuda including HondPoseBarracuda. Please check it.

This consists mainly of a pipeline from two ONNX models converted from TensorFlow Lite model. For more information about them, please check the package in the repository.
- [PoseDetectBarracuda (Detect pose ROI)](https://github.com/torinos-yt/BlazePoseBarracuda/tree/master/Packages/jp.torinos.mediapipe.posedetector)
- [PoseLandmarkBarracuda (Detect pose landmark from ROI)](https://github.com/torinos-yt/BlazePoseBarracuda/tree/master/Packages/jp.torinos.mediapipe.poselandmark)

Require
--------------------------
- Unity 2020

Note
--------------------------
 - In the original implementation of MediaPipe, the ROI is set from the pose landmarks ofthe previous frame, except for the initial frame. This can be expected to run muchfaster. However, in this project, the implementation currently uses both models toperform inference for each frame.
 - Using the `UpperBodyOnly` option, more accurate detection can be expected in situations where a large part of the lower body is occluded.

Install
--------------------------
It can be installed by adding scoped registry to the manifest file(Packages/manifest.json).

`scopedRegistries`
````
{
    "name": "torinos",
    "url": "https://registry.npmjs.com",
    "scopes": ["jp.torinos"]
}
````
`dependencies`
````
"jp.torinos.mediapipe.blazepose": "1.0.0"
````


[MediaPipe Pose]:
  https://google.github.io/mediapipe/solutions/pose.html

[Unity Barracuda]:
  https://docs.unity3d.com/Packages/com.unity.barracuda@latest

[HandPoseBarracuda]:
    https://github.com/keijiro/HandPoseBarracuda

[keijiro]:
    https://github.com/keijiro