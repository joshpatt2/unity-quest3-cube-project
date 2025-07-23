# Quest 3 Mixed Reality App - User Guide

## What's New in This Version

Your Unity VR app has been converted to a **Mixed Reality (MR) app** that blends virtual content with your real environment!

## Features Added

### 🌍 Mixed Reality Support
- **Passthrough Mode**: See your real environment through the headset
- **Virtual Content Overlay**: The cube now appears in your real space
- **Environmental Lighting**: Lighting is optimized for MR viewing

### 🎮 Controls
- **Menu Button**: Toggle between VR mode and MR (passthrough) mode
- **Trigger Button**: Interact with the virtual cube (changes color)
- **The cube is positioned 2 meters in front of you at eye level**

### 🔧 Technical Improvements
- **Android Manifest**: Added MR permissions and capabilities
- **Camera Settings**: Optimized for passthrough rendering
- **XR Settings**: Enabled stage tracking for better room-scale experience

## How to Use

1. **Put on your Quest 3 headset**
2. **Launch the Quest3CubeProject app**
3. **The app starts in MR mode by default** - you should see your real environment with a virtual cube floating in front of you
4. **Press the Menu button** on your controller to toggle between:
   - **MR Mode**: See your real environment + virtual cube
   - **VR Mode**: Traditional black/skybox background + virtual cube

## MR vs VR Mode

### Mixed Reality Mode (Default)
- ✅ See your real environment through passthrough
- ✅ Virtual cube appears to exist in your real space
- ✅ Better depth perception and spatial awareness
- ✅ Can interact with real world while using app

### VR Mode (Traditional)
- ✅ Immersive virtual environment
- ✅ No real-world distractions
- ✅ Traditional VR experience

## Troubleshooting

### If you don't see passthrough:
1. Make sure your Quest 3 has passthrough enabled in system settings
2. Check that the app has camera permissions
3. Try toggling between VR/MR mode using the Menu button

### If the cube appears too close/far:
- The cube is positioned 2 meters away by default
- In MR mode, try moving around to see the cube from different angles
- The cube should appear to be anchored in your real space

## Development Notes

This MR conversion includes:
- **MR-compatible Android manifest** with passthrough permissions
- **Unity XR settings** optimized for Quest 3 MR
- **Passthrough management scripts** for seamless VR/MR switching
- **Environmental lighting** adjusted for mixed reality

The app now supports both VR and MR experiences, giving you the best of both worlds!

---

**Enjoy your new Mixed Reality experience on Quest 3! 🥽✨**
