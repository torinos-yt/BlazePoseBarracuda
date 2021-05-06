PoseDetectBarracuda  
=========================
![img](https://i.imgur.com/EQshTNa.png)

This is pose detector that works with a monocular color camera.  
This works by using the [MediaPipe Pose] model in [Unity Barracuda].

PoseDetectBaracuda is based on [BlazePalmBarracuda] implementation by [keijiro].

ONNX Model
------------------------
The ONNX model files used here have been converted for Unity Barracuda using the same operations as [BlazeFaceBarracuda] by keijiro. Check out [his Colab notebook](https://colab.research.google.com/drive/1O1KDIVsmYyYDqEqv7hEqofsHMCa49xaZ?usp=sharing) for more details.

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
"jp.torinos.mediapipe.posedetector": "1.0.0"
````

[MediaPipe Pose]:
  https://google.github.io/mediapipe/solutions/pose.html

[Unity Barracuda]:
  https://docs.unity3d.com/Packages/com.unity.barracuda@latest

[BlazePalmBarracuda]:
    https://github.com/keijiro/BlazePalmBarracuda

[BlazeFaceBarracuda]:
    https://github.com/keijiro/BlazeFaceBarracuda

[keijiro]:
    https://github.com/keijiro