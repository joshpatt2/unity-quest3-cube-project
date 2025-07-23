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
        
        // Configure Mixed Reality settings for Quest 3
        ConfigureMixedRealitySettings();
        
        Debug.Log("Android settings configured successfully");
    }
    
    private static void ConfigureMixedRealitySettings()
    {
        Debug.Log("Configuring Mixed Reality settings for Quest 3...");
        
        // Enable passthrough capability in Android manifest
        PlayerSettings.Android.forceSDCardPermission = false;
        
        // Add passthrough permission to Android manifest
        var manifestPath = Path.Combine(Application.dataPath, "Plugins", "Android", "AndroidManifest.xml");
        CreateMRAndroidManifest(manifestPath);
        
        // Enable XR for immersive experience
        PlayerSettings.virtualRealitySupported = true;
        
        // Ensure XR is initialized at startup for Android
        ConfigureXRForAndroid();
        
        Debug.Log("Mixed Reality settings configured successfully");
    }
    
    private static void CreateMRAndroidManifest(string manifestPath)
    {
        // Create the directory if it doesn't exist
        Directory.CreateDirectory(Path.GetDirectoryName(manifestPath));
        
        string manifestContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<manifest xmlns:android=""http://schemas.android.com/apk/res/android"">
    
    <!-- Mixed Reality / Passthrough permissions -->
    <uses-permission android:name=""com.oculus.permission.USE_SCENE"" />
    <uses-permission android:name=""com.oculus.permission.HAND_TRACKING"" />
    
    <!-- Camera permission for passthrough -->
    <uses-permission android:name=""android.permission.CAMERA"" />
    
    <!-- VR headset features -->
    <uses-feature android:name=""android.hardware.vr.headtracking"" android:version=""1"" android:required=""true"" />
    <uses-feature android:name=""oculus.software.handtracking"" android:required=""false"" />
    <uses-feature android:name=""com.oculus.feature.PASSTHROUGH"" android:required=""true"" />
    
    <!-- Mark as VR application -->
    <uses-feature android:name=""android.software.vr.mode"" android:required=""true"" />
    <uses-feature android:name=""android.hardware.vr.high_performance"" android:required=""true"" />
    
    <application>
        <!-- VR application metadata -->
        <meta-data android:name=""com.oculus.vr.focusaware"" android:value=""true"" />
        <meta-data android:name=""com.oculus.intent.category.VR"" android:value=""true"" />
        
        <!-- Device support -->
        <meta-data android:name=""com.oculus.supportedDevices"" android:value=""quest|quest2|questpro|quest3"" />
        
        <!-- Enable immersive mode -->
        <meta-data android:name=""unityplayer.UnityActivity"" android:value=""true"" />
        <meta-data android:name=""unity.splash-mode"" android:value=""0"" />
        <meta-data android:name=""unity.splash-enable"" android:value=""False"" />
        
        <!-- Hand tracking configuration -->
        <meta-data android:name=""com.oculus.handtracking.frequency"" android:value=""HIGH"" />
        <meta-data android:name=""com.oculus.handtracking.version"" android:value=""V2.0"" />
        
        <!-- Activity configuration for VR -->
        <activity android:name=""com.unity3d.player.UnityPlayerActivity""
                  android:theme=""@android:style/Theme.Black.NoTitleBar.Fullscreen""
                  android:configChanges=""mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|orientation|screenLayout|uiMode|screenSize|smallestScreenSize|fontScale|layoutDirection|density""
                  android:resizeableActivity=""false""
                  android:screenOrientation=""landscape""
                  android:launchMode=""singleTask""
                  android:exported=""true"">
            
            <!-- VR Intent filters -->
            <intent-filter>
                <action android:name=""android.intent.action.MAIN"" />
                <category android:name=""android.intent.category.LAUNCHER"" />
                <category android:name=""com.oculus.intent.category.VR"" />
            </intent-filter>
        </activity>
    </application>
</manifest>";
        
        File.WriteAllText(manifestPath, manifestContent);
        Debug.Log($"MR Android manifest created at: {manifestPath}");
    }
    
    private static void ConfigureXRForAndroid()
    {
        Debug.Log("Configuring XR settings for immersive Android experience...");
        
        // Update XR General Settings to include Android
        UpdateXRGeneralSettings();
        
        // Force XR initialization
        EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.Generic;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        
        Debug.Log("XR configured for Android immersive experience");
    }
    
    private static void UpdateXRGeneralSettings()
    {
        // This ensures XR is enabled for Android builds
        string xrSettingsContent = @"%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!114 &-8049147911962959679
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 0}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {fileID: 11500000, guid: f4c3631f5e58749a59194e0cf6baf6d5, type: 3}
  m_Name: Standalone Providers
  m_EditorClassIdentifier: 
  m_RequiresSettingsUpdate: 0
  m_AutomaticLoading: 0
  m_AutomaticRunning: 0
  m_Loaders:
  - {fileID: 11400000, guid: d5cdda7b3fb4d45a8b00c170c3f9106d, type: 2}
--- !u!114 &1
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 0}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {fileID: 11500000, guid: f4c3631f5e58749a59194e0cf6baf6d5, type: 3}
  m_Name: Android Providers
  m_EditorClassIdentifier: 
  m_RequiresSettingsUpdate: 0
  m_AutomaticLoading: 1
  m_AutomaticRunning: 1
  m_Loaders:
  - {fileID: 11400000, guid: d5cdda7b3fb4d45a8b00c170c3f9106d, type: 2}
--- !u!114 &11400000
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 0}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {fileID: 11500000, guid: d2dc886499c26824283350fa532d087d, type: 3}
  m_Name: XRGeneralSettingsPerBuildTarget
  m_EditorClassIdentifier: 
  Keys: 010000000700000013000000
  Values:
  - {fileID: 8171922050606877716}
  - {fileID: 1}
  - {fileID: 1}
--- !u!114 &8171922050606877716
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 0}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {fileID: 11500000, guid: d236b7d11115f2143951f1e14045df39, type: 3}
  m_Name: Standalone Settings
  m_EditorClassIdentifier: 
  m_LoaderManagerInstance: {fileID: -8049147911962959679}
  m_InitManagerOnStart: 1";
  
        string xrSettingsPath = Path.Combine(Application.dataPath, "XR", "XRGeneralSettingsPerBuildTarget.asset");
        File.WriteAllText(xrSettingsPath, xrSettingsContent);
        Debug.Log("XR General Settings updated for Android");
    }
}
