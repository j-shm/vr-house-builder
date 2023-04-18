# VR House builder in unity using mrtk3

## Aim

The aim of this project is to create a vr game that allows for the basic mechanics of a vr building system including: vr movement, windows, walls, objects (movable in vr) and models must be dynamically loaded into the game.

## Requirements

- [Unity](https://unity.com/) 
- [MRTK3](https://github.com/microsoft/MixedRealityToolkit-Unity/tree/mrtk3) for VR support
- [gLTFast](https://github.com/atteneder/glTFast) for model importing and required by mrtk3
- [Newtonsoft Json](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@2.0/manual/index.html) for reading object json infomation files
- [pb_CSG](https://github.com/karl-/pb_CSG) for csg to cut out windows from walls

## Installing

Install Unity and download Microsoft Mixed Reality Feature Tool and install the modules required

Modules:

- MRTK3
    - ALL 
    - Only Version 3.0.0-pre.13 has been tested but it may work in later versions with slight modification or none at all
- Platform Support
    - Mixed Reality OpenXR Plugin
    - Tested on Version 1.7.0

Then install [Newtonsoft Json](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@2.0/manual/index.html) (Tested on Version 3.0.2) and [pb_CSG](https://github.com/karl-/pb_CSG) (Tested on Version 2.0.0). 

This should allow the project to open and work fine. You can complie as you would any other unity project (quest compling has not been tested however.)