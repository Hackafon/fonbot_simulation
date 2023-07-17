# FONBot Simulation
Unity simulation for FONBot - assistant robot made at FON

## Setup
To get started, install Unity Hub and Unity 2020.3.30f1 (make sure that you install that specific version of Unity) on your machine. 
Once you have Unity installed, clone the repository.<br>

## Unity <-> ROS2 integration
For us to be able to integrate our Unity simulation with ROS2, we are using [ROS2 To Unity](https://github.com/RobotecAI/ros2-for-unity) package.

## Opening Unity project
From your ROS2 terminal type the following command:
- ```<path to unity installation folder\Unity.exe> -projectPath <project path>```

## Unity Editor
Unity Editor consists of multiple views:

<details>
  
<summary>Scene view</summary>

<br>Scene view allows you to edit and navigate through active Unity scene. 
For more info on scene view navigation check <a href="https://docs.unity3d.com/Manual/SceneViewNavigation.html">Unity documentation</a>. <br><br>
<image src="https://i.imgur.com/aBGtWOW.png" />
</details>

<details>
  
<summary>Game view</summary>

<br>Game view shows how the final, built application looks. 
For more info on game view check <a href="https://docs.unity3d.com/Manual/GameView.html">Unity documentation</a>. <br><br>
<image src="https://i.imgur.com/9wSX578.png" />
</details>

<details>
  
<summary>Project view</summary>

<br>Project view shows all the files in your project.<br>
Folder structure of our project is:
```
├───Graphics
│   ├───Environment
│   │   ├───closet
│   │   │   └───source
│   │   ├───low-poly-computer-desk
│   │   │   ├───source
│   │   │   │   └───LowPolyComputerDesk
│   │   │   └───textures
│   │   ├───LowPolyOfficeProps_LITE
│   │   │   ├───LowPolyOfficeProps_LiteScene
│   │   │   ├───Materials
│   │   │   ├───Meshes
│   │   │   │   └───Materials
│   │   │   ├───PostProcessProfiles
│   │   │   ├───Prefabs
│   │   │   └───Textures
│   │   ├───Office Supplies Low Poly
│   │   │   └───Assets
│   │   │       ├───FBX
│   │   │       │   └───Materials
│   │   │       ├───Materials
│   │   │       ├───Prefabs
│   │   │       ├───Sample scene
│   │   │       └───Textures
│   │   └───PolygonStarter
│   │       ├───Materials
│   │       │   ├───Misc
│   │       │   └───Plane
│   │       ├───Models
│   │       │   └───Collision
│   │       ├───PolygonPrototype
│   │       │   ├───Materials
│   │       │   │   ├───FX
│   │       │   │   ├───Misc
│   │       │   │   └───Triplanar_ObjectSpace
│   │       │   ├───Models
│   │       │   │   └───Materials
│   │       │   ├───Prefabs
│   │       │   │   ├───Buildings
│   │       │   │   │   ├───Polygon
│   │       │   │   │   └───Simple
│   │       │   │   ├───Characters
│   │       │   │   ├───Character_FPS_Hands
│   │       │   │   │   ├───FiveFinger
│   │       │   │   │   └───Standard
│   │       │   │   ├───Character_Pawns
│   │       │   │   ├───FX
│   │       │   │   ├───Generic
│   │       │   │   ├───Icons
│   │       │   │   ├───Other
│   │       │   │   ├───Primitives
│   │       │   │   ├───Props
│   │       │   │   ├───Vehicle
│   │       │   │   └───_NewAssets
│   │       │   ├───Scenes
│   │       │   ├───Shaders
│   │       │   └───Textures
│   │       ├───Prefabs
│   │       │   └───Characters
│   │       ├───Scenes
│   │       └───Textures
│   ├───Textures
│   │   └───rusty-panel-unity
│   └───URDF
│       └───description
│           ├───fonbot_package
│           │   └───models
│           ├───Materials
│           └───meshes
├───Prefabs
├───Resources
├───Ros2ForUnity
│   ├───Plugins
│   │   └───Windows
│   │       └───x86_64
│   └───Scripts
│       └───Time
└───Scenes
    └───MainScene_Profiles
```
<br>
<image src="https://i.imgur.com/c85lsAy.png" />
</details>

<details>
  
<summary>Hierarchy</summary>

<br>Hierarchy shows all objects in your scene and their hierarchical relationship. <br><br>
<image src="https://i.imgur.com/x2lMe68.png" />
</details>

## Opening Unity scene
Once you've opened Unity project, you can open main scene by going into \Scenes folder and double clicking on ```MainScene.unity```.

## Running Unity simulation
You can run your simulation by clicking on ▶ (play) button at the top of Unity Editor. 
You can also pause the simulation and step by frame (buttons are located at the same place as the play button).
