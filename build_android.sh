#!/bin/bash

# Unity Quest 3 Build Script
# This script builds the Quest 3 Cube Project for Android

echo "=== Unity Quest 3 Build Script ==="
echo "Setting up environment variables..."

# Set environment variables for Android development (using Unity's built-in tools)
export JAVA_HOME="/Applications/Unity/Hub/Editor/6000.0.53f1/PlaybackEngines/AndroidPlayer/OpenJDK"
export ANDROID_SDK_ROOT="/Applications/Unity/Hub/Editor/6000.0.53f1/PlaybackEngines/AndroidPlayer/SDK"
export ANDROID_NDK_ROOT="/Applications/Unity/Hub/Editor/6000.0.53f1/PlaybackEngines/AndroidPlayer/NDK"
export PATH="$ANDROID_SDK_ROOT/platform-tools:$PATH"

echo "JAVA_HOME: $JAVA_HOME"
echo "ANDROID_SDK_ROOT: $ANDROID_SDK_ROOT"
echo "ANDROID_NDK_ROOT: $ANDROID_NDK_ROOT"

# Check if JDK exists
if [ ! -d "$JAVA_HOME" ]; then
    echo "❌ ERROR: JDK not found at $JAVA_HOME"
    echo "Please install Java 11 or configure JAVA_HOME"
    exit 1
fi

# Check if Android SDK exists
if [ ! -d "$ANDROID_SDK_ROOT" ]; then
    echo "❌ ERROR: Android SDK not found at $ANDROID_SDK_ROOT"
    echo "Please install Android SDK or configure ANDROID_SDK_ROOT"
    exit 1
fi

echo "✅ Environment validation passed"
echo ""
echo "Starting Unity build..."

# Unity build command
/Applications/Unity/Hub/Editor/6000.0.53f1/Unity.app/Contents/MacOS/Unity \
    -batchmode \
    -quit \
    -projectPath "$(pwd)" \
    -buildTarget Android \
    -executeMethod SimpleBuildScript.BuildAndroid \
    -logFile build_script.log

# Check if build was successful
if [ $? -eq 0 ]; then
    echo ""
    echo "🔍 Checking for build output..."
    
    if [ -f "Builds/Quest3CubeProject.apk" ]; then
        echo "✅ BUILD SUCCESSFUL!"
        echo "📱 APK created: Builds/Quest3CubeProject.apk"
        ls -la "Builds/Quest3CubeProject.apk"
    else
        echo "⚠️  Unity exited successfully but APK not found"
        echo "Check build_script.log for details"
    fi
else
    echo "❌ BUILD FAILED"
    echo "Check build_script.log for details"
    echo ""
    echo "Last 20 lines of build log:"
    tail -20 build_script.log
fi

echo ""
echo "=== Build script completed ==="
