# Unity Project Instructions for GitHub Copilot

<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->

This is a Unity 3D project configured for Meta Quest 3 VR development. When suggesting code:

## Project Context
- Unity version: 2023.3.18f1
- Target Platform: Android (Meta Quest 3)
- VR SDK: Oculus/Meta SDK
- Rendering: Mobile optimized for VR

## Code Guidelines
- Use Unity C# scripting conventions
- Follow MonoBehaviour patterns for Unity scripts
- Consider VR performance optimization (90Hz target framerate)
- Use appropriate VR-specific APIs for Quest 3
- Implement hand tracking and controller input support when relevant
- Consider spatial audio and haptic feedback

## Common Tasks
- Creating VR-ready scenes with XR Rig
- Implementing VR interaction systems
- Optimizing for mobile VR performance
- Setting up hand tracking and controller support
- Building and deploying to Quest 3 devices

## File Structure
- Scripts should go in Assets/Scripts/
- Scenes should go in Assets/Scenes/
- Materials and textures should be optimized for mobile VR
- Use appropriate texture compression for Android

## Performance Considerations
- Target 90 FPS for smooth VR experience
- Use efficient rendering techniques
- Minimize draw calls and overdraw
- Use appropriate LOD systems
- Consider fixed foveated rendering
