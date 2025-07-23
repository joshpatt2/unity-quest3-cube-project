using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using System.Collections;

public class XRImmersiveInitializer : MonoBehaviour
{
    [Header("XR Initialization")]
    public bool initializeXROnStart = true;
    public bool enablePassthroughOnStart = true;
    
    [Header("Debug")]
    public bool showDebugLogs = true;
    
    private void Start()
    {
        if (initializeXROnStart)
        {
            StartCoroutine(InitializeXR());
        }
    }
    
    private IEnumerator InitializeXR()
    {
        LogDebug("Starting XR initialization for immersive experience...");
        
        // Check if XR is supported
        if (!XRGeneralSettings.Instance)
        {
            LogError("XRGeneralSettings.Instance is null! XR may not be properly configured.");
            yield break;
        }
        
        var xrManagerSettings = XRGeneralSettings.Instance.Manager;
        if (!xrManagerSettings)
        {
            LogError("XR Manager Settings not found!");
            yield break;
        }
        
        // Initialize XR
        yield return xrManagerSettings.InitializeLoader();
        
        if (xrManagerSettings.activeLoader == null)
        {
            LogError("Failed to initialize XR. No active loader found.");
            yield break;
        }
        
        LogDebug($"XR Loader initialized: {xrManagerSettings.activeLoader.name}");
        
        // Start XR
        xrManagerSettings.StartSubsystems();
        LogDebug("XR subsystems started successfully!");
        
        // Wait a frame for XR to fully initialize
        yield return null;
        
        // Configure for immersive experience
        ConfigureImmersiveSettings();
        
        // Enable passthrough if requested
        if (enablePassthroughOnStart)
        {
            EnablePassthrough();
        }
        
        LogDebug("XR initialization complete - App should now be immersive!");
    }
    
    private void ConfigureImmersiveSettings()
    {
        LogDebug("Configuring immersive settings...");
        
        // Ensure the main camera is set up for XR
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // XR will handle camera positioning, but we need to ensure proper settings
            mainCamera.nearClipPlane = 0.01f;
            mainCamera.farClipPlane = 1000f;
            
            LogDebug("Main camera configured for XR");
        }
        
        // Set tracking origin mode for room-scale experience
        var inputSubsystem = GetInputSubsystem();
        if (inputSubsystem != null)
        {
            if (inputSubsystem.TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor))
            {
                LogDebug("Tracking origin set to Floor (room-scale)");
            }
            else if (inputSubsystem.TrySetTrackingOriginMode(TrackingOriginModeFlags.Device))
            {
                LogDebug("Tracking origin set to Device (seated)");
            }
        }
    }
    
    private void EnablePassthrough()
    {
        LogDebug("Attempting to enable passthrough for MR...");
        
        // Set camera background to transparent for passthrough
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = Color.clear;
            LogDebug("Camera configured for passthrough");
        }
        
        // Disable skybox for passthrough
        RenderSettings.skybox = null;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = Color.white * 0.3f;
        
        LogDebug("Passthrough environment configured");
    }
    
    private XRInputSubsystem GetInputSubsystem()
    {
        var inputSubsystems = new System.Collections.Generic.List<XRInputSubsystem>();
        SubsystemManager.GetInstances(inputSubsystems);
        
        if (inputSubsystems.Count > 0)
        {
            return inputSubsystems[0];
        }
        
        return null;
    }
    
    private void LogDebug(string message)
    {
        if (showDebugLogs)
        {
            Debug.Log($"[XRImmersiveInitializer] {message}");
        }
    }
    
    private void LogError(string message)
    {
        Debug.LogError($"[XRImmersiveInitializer] {message}");
    }
    
    private void OnApplicationPause(bool pauseStatus)
    {
        // Handle app pause/resume for XR
        if (!pauseStatus) // App resumed
        {
            LogDebug("App resumed - checking XR state");
            var xrManagerSettings = XRGeneralSettings.Instance?.Manager;
            if (xrManagerSettings != null && xrManagerSettings.activeLoader == null)
            {
                StartCoroutine(InitializeXR());
            }
        }
    }
    
    private void OnDestroy()
    {
        // Cleanup XR when destroyed
        var xrManagerSettings = XRGeneralSettings.Instance?.Manager;
        if (xrManagerSettings != null && xrManagerSettings.activeLoader != null)
        {
            LogDebug("Cleaning up XR...");
            xrManagerSettings.StopSubsystems();
            xrManagerSettings.DeinitializeLoader();
        }
    }
}
