# Unity Quest 3 Cube Project

A Unity 3D project configured for Meta Quest 3 VR development featuring a simple rotating cube scene.

![Unity](https://img.shields.io/badge/Unity-6000.0.53f1-black)
![Platform](https://img.shields.io/badge/Platform-Meta%20Quest%203-blue)
![VR](https://img.shields.io/badge/VR-Ready-green)

## 🎯 Project Overview

This project demonstrates a basic Unity setup for Meta Quest 3 VR development with:
- 🔄 A simple scene containing a rotating cube
- 🥽 VR-optimized project settings  
- 📱 Android build configuration for Quest 3
- 💡 Basic lighting and camera setup
- 🛠️ Automated build scripts

## 🚀 Quick Start

### Prerequisites
- Unity 2023.3.18f1+ with Android Build Support
- Meta Quest 3 with Developer Mode enabled
- USB cable for deployment

### Build & Deploy
1. Clone this repository
2. Open project in Unity Hub
3. Configure External Tools (see [BUILD_INSTRUCTIONS.md](BUILD_INSTRUCTIONS.md))
4. Build: **Build → Build for Quest 3**
5. Deploy: Connect Quest 3 and run deployment script

## 📁 Project Structure

```
Assets/
├── Scenes/
│   └── SampleScene.unity      # Main scene with rotating cube
└── Scripts/
    ├── CubeRotator.cs         # Cube rotation logic
    ├── VRSetupHelper.cs       # VR configuration helper
    └── Editor/
        ├── SimpleBuildScript.cs    # Automated build script
        └── Quest3Builder.cs        # Advanced build configuration

.vscode/                       # VS Code tasks and settings
├── tasks.json                 # Build and deploy tasks

Build Scripts/                 # Automated build tools
├── build_android.sh           # Shell script for building
└── BUILD_INSTRUCTIONS.md      # Detailed setup guide
```

## ✨ Features

- 🎮 **VR Ready**: Pre-configured for Meta Quest 3
- 🔄 **Interactive Cube**: Rotating cube with physics
- 🛠️ **Build Automation**: One-click build and deploy scripts
- 📱 **Android Optimized**: Performance optimized for mobile VR
- 🎯 **XR Toolkit**: Includes XR Interaction Toolkit setup
- 🔧 **Developer Tools**: VR setup validation and debugging tools

## 📋 Build Instructions

**Quick Build:**
```bash
./build_android.sh
```

**Detailed Instructions:**
See [BUILD_INSTRUCTIONS.md](BUILD_INSTRUCTIONS.md) for complete setup guide.

**VS Code Tasks:**
- `Ctrl+Shift+P` → "Tasks: Run Task" → "Build for Quest 3"
- `Ctrl+Shift+P` → "Tasks: Run Task" → "Build and Deploy to Quest 3"

## 🎮 Scripts

### CubeRotator.cs
```csharp
public class CubeRotator : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 50, 0);
    
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
```

### VRSetupHelper.cs
Validates VR configuration and provides debugging information in the console.

## 🔧 Development

### Prerequisites
- Unity 6000.0.53f1 with Android Build Support
- Java 17 (automatically configured)
- Android SDK/NDK (Unity built-in)

### Building
1. **Configure Unity External Tools** (one-time setup)
2. **Build:** Use Unity menu or automated scripts
3. **Deploy:** Connect Quest 3 via USB and deploy

## 📚 Resources

- [Unity XR Documentation](https://docs.unity3d.com/Manual/XR.html)
- [Meta Quest Developer Docs](https://developer.oculus.com/documentation/unity/)
- [XR Interaction Toolkit](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@latest)

## 📄 License

This project is created for educational purposes by Josh Patterson. Unity and Meta Quest are trademarks of their respective owners.

## 👨‍💻 Author

**Josh Patterson**  
📧 joshpatterson@outlook.com

---

**Ready to build your first VR experience for Quest 3? 🚀**
