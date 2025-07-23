using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;

public class Quest3Builder
{
    [MenuItem("Build/Build for Quest 3")]
    public static void BuildForQuest3()
    {
        // Set build target to Android
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        
        // Configure Android settings
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel29;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        
        // Set VR settings
        PlayerSettings.virtualRealitySupported = true;
        
        // Set product name and company
        PlayerSettings.companyName = "Josh Patterson";
        PlayerSettings.productName = "Quest3CubeProject";
        
        // Get scenes to build
        string[] scenes = GetEnabledScenes();
        
        // Set build path
        string buildPath = Path.Combine(Application.dataPath, "..", "Builds", "Quest3CubeProject.apk");
        Directory.CreateDirectory(Path.GetDirectoryName(buildPath));
        
        // Build options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = buildPath;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;
        
        // Start build
        Debug.Log("Starting build for Quest 3...");
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Build succeeded! APK created at: {buildPath}");
            Debug.Log($"Build size: {summary.totalSize} bytes");
            EditorUtility.RevealInFinder(buildPath);
        }
        else
        {
            Debug.LogError($"Build failed: {summary.result}");
            foreach (BuildStep step in report.steps)
            {
                foreach (BuildStepMessage message in step.messages)
                {
                    if (message.type == LogType.Error || message.type == LogType.Exception)
                    {
                        Debug.LogError($"Build Error: {message.content}");
                    }
                }
            }
        }
    }
    
    private static string[] GetEnabledScenes()
    {
        System.Collections.Generic.List<string> scenes = new System.Collections.Generic.List<string>();
        
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                scenes.Add(scene.path);
            }
        }
        
        // If no scenes are enabled, add the sample scene
        if (scenes.Count == 0)
        {
            scenes.Add("Assets/Scenes/SampleScene.unity");
        }
        
        return scenes.ToArray();
    }
}
