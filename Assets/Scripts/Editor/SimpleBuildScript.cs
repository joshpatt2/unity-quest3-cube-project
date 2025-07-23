using UnityEngine;
using UnityEditor;
using System.IO;

public class SimpleBuildScript
{
    public static void BuildAndroid()
    {
        Debug.Log("Starting build for Android...");
        
        // Configure Android SDK and JDK paths
        ConfigureAndroidBuildEnvironment();
        
        // Switch to Android platform
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        
        // Configure Android-specific settings
        ConfigureAndroidSettings();
        
        // Get scenes
        var scenes = new string[] { "Assets/Scenes/SampleScene.unity" };
        
        // Create build directory
        string buildPath = "Builds/Quest3CubeProject.apk";
        Directory.CreateDirectory(Path.GetDirectoryName(buildPath));
        
        // Build
        var buildReport = UnityEditor.BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.Android, BuildOptions.None);
        
        if (buildReport.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log($"Build completed successfully! APK created at: {buildPath}");
            Debug.Log($"Build size: {buildReport.summary.totalSize} bytes");
        }
        else
        {
            Debug.LogError($"Build failed: {buildReport.summary.result}");
        }
    }
    
    private static void ConfigureAndroidBuildEnvironment()
    {
        Debug.Log("Configuring Android build environment...");
        
        // Set JDK path (Unity's built-in Java 17)
        string jdkPath = "/Applications/Unity/Hub/Editor/6000.0.53f1/PlaybackEngines/AndroidPlayer/OpenJDK";
        if (Directory.Exists(jdkPath))
        {
            UnityEditor.EditorPrefs.SetString("JdkPath", jdkPath);
            Debug.Log($"JDK path set to: {jdkPath}");
        }
        else
        {
            Debug.LogWarning($"JDK path not found: {jdkPath}");
        }
        
        // Set Android SDK path (Unity's built-in SDK)
        string androidSdkPath = "/Applications/Unity/Hub/Editor/6000.0.53f1/PlaybackEngines/AndroidPlayer/SDK";
        if (Directory.Exists(androidSdkPath))
        {
            UnityEditor.EditorPrefs.SetString("AndroidSdkRoot", androidSdkPath);
            Debug.Log($"Android SDK path set to: {androidSdkPath}");
        }
        else
        {
            Debug.LogWarning($"Android SDK path not found: {androidSdkPath}");
        }
        
        // Set Android NDK path (Unity's built-in NDK)
        string androidNdkPath = "/Applications/Unity/Hub/Editor/6000.0.53f1/PlaybackEngines/AndroidPlayer/NDK";
        if (Directory.Exists(androidNdkPath))
        {
            UnityEditor.EditorPrefs.SetString("AndroidNdkRoot", androidNdkPath);
            Debug.Log($"Android NDK path set to: {androidNdkPath}");
        }
        else
        {
            Debug.LogWarning($"Android NDK path not found: {androidNdkPath}");
        }
    }
    
    private static void ConfigureAndroidSettings()
    {
        Debug.Log("Configuring Android-specific settings...");
        
        // Set minimum and target SDK versions
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26; // API 26 (Android 8.0)
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel33; // API 33 (Android 13)
        
        // Set scripting backend to IL2CPP (required for Quest) - using older API for compatibility
        #pragma warning disable CS0618
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        #pragma warning restore CS0618
        
        // Set target architecture to ARM64 (required for Quest)
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        
        // Set product name and company
        PlayerSettings.companyName = "Josh Patterson";
        PlayerSettings.productName = "Quest3CubeProject";
        PlayerSettings.applicationIdentifier = "com.JoshPatterson.Quest3CubeProject";
        
        Debug.Log("Android settings configured successfully");
    }
}
