using UnityEngine;
using UnityEditor;
using UnityEngine.XR.Management;
using UnityEditor.XR.Management;

namespace Quest3VR.Editor
{
    /// <summary>
    /// Editor utility to fix XR Management settings for Quest 3 VR development
    /// </summary>
    public class XRSettingsFixer : EditorWindow
    {
        [MenuItem("Quest3VR/Fix XR Settings")]
        public static void ShowWindow()
        {
            var window = GetWindow<XRSettingsFixer>("XR Settings Fixer");
            window.Show();
        }
        
        private void OnGUI()
        {
            GUILayout.Label("XR Settings Fixer for Quest 3", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            GUILayout.Label("This tool helps resolve XR Management NullReference exceptions.", EditorStyles.helpBox);
            GUILayout.Space(10);
            
            if (GUILayout.Button("Check XR Settings", GUILayout.Height(30)))
            {
                CheckXRSettings();
            }
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("Reinitialize XR Settings", GUILayout.Height(30)))
            {
                ReinitializeXRSettings();
            }
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("Force Create XR General Settings", GUILayout.Height(30)))
            {
                ForceCreateXRGeneralSettings();
            }
            
            GUILayout.Space(10);
            
            GUILayout.Label("Manual Steps:", EditorStyles.boldLabel);
            GUILayout.Label("1. Open Edit → Project Settings → XR Plug-in Management", EditorStyles.wordWrappedLabel);
            GUILayout.Label("2. Ensure 'Oculus' is checked for Android platform", EditorStyles.wordWrappedLabel);
            GUILayout.Label("3. Restart Unity Editor if issues persist", EditorStyles.wordWrappedLabel);
        }
        
        private void CheckXRSettings()
        {
            Debug.Log("=== XR Settings Check ===");
            
            // Check if XRGeneralSettings exists
            var xrGeneralSettings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Android);
            if (xrGeneralSettings == null)
            {
                Debug.LogError("XRGeneralSettings for Android is NULL!");
            }
            else
            {
                Debug.Log("XRGeneralSettings for Android exists");
                
                if (xrGeneralSettings.Manager == null)
                {
                    Debug.LogError("XRManagerSettings is NULL!");
                }
                else
                {
                    Debug.Log($"XRManagerSettings exists with {xrGeneralSettings.Manager.activeLoaders.Count} loaders");
                    foreach (var loader in xrGeneralSettings.Manager.activeLoaders)
                    {
                        Debug.Log($"  - Active Loader: {loader.name}");
                    }
                }
            }
            
            // Check for Standalone as well
            var xrGeneralSettingsStandalone = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Standalone);
            if (xrGeneralSettingsStandalone != null)
            {
                Debug.Log("XRGeneralSettings for Standalone exists");
            }
            else
            {
                Debug.LogWarning("XRGeneralSettings for Standalone is NULL");
            }
        }
        
        private void ReinitializeXRSettings()
        {
            Debug.Log("Reinitializing XR Settings...");
            
            try
            {
                // Force reload of XR settings
                AssetDatabase.Refresh();
                
                // Try to get XR settings for Android
                var buildTargetGroup = BuildTargetGroup.Android;
                var xrGeneralSettings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(buildTargetGroup);
                
                if (xrGeneralSettings == null)
                {
                    Debug.LogWarning("Creating new XRGeneralSettings for Android...");
                    
                    // Create new XR settings if they don't exist
                    var settings = ScriptableObject.CreateInstance<XRGeneralSettings>();
                    var managerSettings = ScriptableObject.CreateInstance<XRManagerSettings>();
                    settings.Manager = managerSettings;
                    
                    // Note: API changed in newer Unity versions
                    Debug.Log("XRGeneralSettings API has changed - manual configuration required");
                    Debug.Log("Please configure XR settings manually in Project Settings > XR Plug-in Management");
                }
                
                // EditorUtility.SetDirty(XRGeneralSettingsPerBuildTarget.Instance);
                AssetDatabase.SaveAssets();
                
                Debug.Log("XR Settings reinitialization complete");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to reinitialize XR Settings: {e.Message}");
            }
        }
        
        private void ForceCreateXRGeneralSettings()
        {
            Debug.Log("Force creating XR General Settings...");
            
            try
            {
                // Ensure the XR folder exists
                string xrFolderPath = "Assets/XR";
                if (!AssetDatabase.IsValidFolder(xrFolderPath))
                {
                    AssetDatabase.CreateFolder("Assets", "XR");
                }
                
                // Create XR General Settings if it doesn't exist
                string settingsPath = "Assets/XR/XRGeneralSettingsPerBuildTarget.asset";
                var existingSettings = AssetDatabase.LoadAssetAtPath<XRGeneralSettingsPerBuildTarget>(settingsPath);
                
                if (existingSettings == null)
                {
                    var newSettings = ScriptableObject.CreateInstance<XRGeneralSettingsPerBuildTarget>();
                    AssetDatabase.CreateAsset(newSettings, settingsPath);
                    Debug.Log("Created new XRGeneralSettingsPerBuildTarget asset");
                }
                
                // Ensure Loaders folder exists
                string loadersPath = "Assets/XR/Loaders";
                if (!AssetDatabase.IsValidFolder(loadersPath))
                {
                    AssetDatabase.CreateFolder("Assets/XR", "Loaders");
                }
                
                // Create Oculus Loader if it doesn't exist
                string oculusLoaderPath = "Assets/XR/Loaders/OculusLoader.asset";
                var existingOculusLoader = AssetDatabase.LoadAssetAtPath(oculusLoaderPath, typeof(ScriptableObject));
                
                if (existingOculusLoader == null)
                {
                    // Try to find the Oculus Loader type
                    var oculusLoaderType = System.Type.GetType("Unity.XR.Oculus.OculusLoader, Unity.XR.Oculus");
                    if (oculusLoaderType != null)
                    {
                        var oculusLoader = ScriptableObject.CreateInstance(oculusLoaderType);
                        AssetDatabase.CreateAsset(oculusLoader, oculusLoaderPath);
                        Debug.Log("Created new OculusLoader asset");
                    }
                    else
                    {
                        Debug.LogWarning("Could not find OculusLoader type. Make sure Oculus XR package is properly installed.");
                    }
                }
                
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                
                Debug.Log("Force creation of XR General Settings complete");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to force create XR General Settings: {e.Message}");
            }
        }
    }
}
