PoseLandmarkBarracuda  
=========================
![img](https://i.imgur.com/r8U9iJR.png)

This is pose landmark detector that works with a monocular color camera.  
This works by using the [MediaPipe Pose] model in [Unity Barracuda].  
 It also includes a simple segmentation function.

PoseLandmarkBaracuda is based on [HandLandmarkBarracuda] implementation by [keijiro].

ONNX Model
------------------------
The ONNX model files used here have been converted for Unity Barracuda using [tf2onnx](https://github.com/onnx/tensorflow-onnx) `--opset=9`, and the same kind of operation as keijiro's [BlazeFaceBarracuda]. See [his Colab notebook](https://colab.research.google.com/drive/1O1KDIVsmYyYDqEqv7hEqofsHMCa49xaZ?usp=sharing) for details.

Require
--------------------------
- Unity 2020

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
"jp.torinos.mediapipe.poselandmark": "1.0.0"
````

[MediaPipe Pose]:
  https://google.github.io/mediapipe/solutions/pose.html

[Unity Barracuda]:
  https://docs.unity3d.com/Packages/com.unity.barracuda@latest

[HandLandmarkBarracuda]:
  https://github.com/keijiro/HandLandmarkBarracuda

[BlazeFaceBarracuda]:
  https://github.com/keijiro/BlazeFaceBarracuda

[keijiro]:
  https://github.com/keijiro