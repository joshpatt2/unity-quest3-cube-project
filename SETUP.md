# Unity Quest 3 Project - Detailed Setup Guide

## 📋 Prerequisites

### Unity Development Environment
- **Unity Hub**: Download from [unity.com](https://unity.com)
- **Unity 6000.0.53f1**: Install through Unity Hub
- **Android Build Support**: Install via Unity Hub modules

### Meta Quest 3 Development
- **Meta Quest Developer Hub**: [Download here](https://developer.oculus.com/downloads/package/oculus-developer-hub-mac/)
- **Developer Mode**: Enable on Quest 3 device
- **USB Debugging**: Enable in Quest 3 settings

## 🛠️ Setup Steps

### 1. Unity Installation
```bash
# Open Unity Hub and install Unity 6000.0.53f1
# Make sure to include Android Build Support module
```

### 2. Project Setup
```bash
# Clone or download this repository
git clone <repository-url>
cd UnityProject

# Open project in Unity Hub
```

### 3. Configure Unity External Tools

**Unity → Preferences → External Tools**

Set these paths:
- **JDK**: `/Applications/Unity/Hub/Editor/6000.0.53f1/PlaybackEngines/AndroidPlayer/OpenJDK`
- **Android SDK**: `/Applications/Unity/Hub/Editor/6000.0.53f1/PlaybackEngines/AndroidPlayer/SDK`
- **Android NDK**: `/Applications/Unity/Hub/Editor/6000.0.53f1/PlaybackEngines/AndroidPlayer/NDK`

### 4. Quest 3 Setup
1. Enable **Developer Mode** in Meta Quest app
2. Connect Quest 3 via USB
3. Enable **USB Debugging** when prompted
4. Accept debugging authorization on headset

### 5. Verify Setup
```bash
# Check ADB connection
adb devices

# Should show your Quest 3 device
```

## 🚀 Building

### Method 1: Unity GUI
1. **File → Build Settings**
2. **Platform: Android**
3. **Build** or **Build and Run**

### Method 2: VS Code Tasks
1. `Ctrl+Shift+P` → "Tasks: Run Task"
2. Select "Build for Quest 3"

### Method 3: Command Line
```bash
./build_android.sh
```

## 🔧 Advanced Configuration

### XR Settings
- **Edit → Project Settings → XR Plug-in Management**
- **Enable Oculus** for Android platform
- **Initialize XR on Startup**: Checked

### Player Settings
- **Company Name**: Set your company name
- **Product Name**: Quest3CubeProject
- **Minimum API Level**: 26 (Android 8.0)
- **Target API Level**: 33 (Android 13)
- **Scripting Backend**: IL2CPP
- **Target Architectures**: ARM64

### Quality Settings
- **Texture Quality**: Fast
- **Anisotropic Textures**: Per Texture
- **Anti Aliasing**: Disabled (for VR performance)
- **Soft Particles**: Disabled
- **Shadows**: Hard Shadows Only

## 🐛 Troubleshooting

### Common Issues

**"JDK not found"**
- Use Unity's built-in JDK path
- Restart Unity after setting External Tools

**"NDK not found"**
- Use Unity's built-in NDK path
- Verify Android Build Support is installed

**"Device not found"**
- Check USB connection
- Enable USB Debugging on Quest 3
- Run `adb devices` to verify connection

**Build fails**
- Check all External Tools paths are set
- Restart Unity Editor
- Clean build folder and try again

**VR not working**
- Verify Oculus plugin is enabled
- Check XR settings in Project Settings
- Ensure Quest 3 is in Developer Mode

### Performance Issues
- Reduce texture resolution
- Disable expensive visual effects
- Use Single Pass rendering
- Enable Fixed Foveated Rendering

## 📚 Next Steps

After successful build and deployment:

1. **Add VR Interactions**
   - Implement hand tracking
   - Add grabbable objects
   - Create UI interactions

2. **Enhance Graphics**
   - Add better lighting
   - Implement URP (Universal Render Pipeline)
   - Add post-processing effects

3. **Add Audio**
   - Spatial audio setup
   - Audio sources for interactions
   - Background music/ambience

4. **Performance Optimization**
   - Profile with Unity Profiler
   - Optimize draw calls
   - Implement LOD systems

## 🔗 Useful Links

- [Unity XR Best Practices](https://docs.unity3d.com/Manual/xr-best-practices.html)
- [Meta Quest Development Guide](https://developer.oculus.com/documentation/unity/unity-gs-overview/)
- [XR Interaction Toolkit Samples](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@latest)
- [Unity Performance Optimization](https://docs.unity3d.com/Manual/BestPracticeGuides.html)

---

Need help? Check the [Issues](../../issues) page or create a new issue with your problem details.
