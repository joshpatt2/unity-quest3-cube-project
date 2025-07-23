using UnityEngine;

public class VRSetupHelper : MonoBehaviour
{
    [Header("VR Setup Helper")]
    [Tooltip("Run this script to help verify VR setup")]
    public bool logVRStatus = true;
    
    void Start()
    {
        if (logVRStatus)
        {
            CheckVRSetup();
        }
    }
    
    void CheckVRSetup()
    {
        Debug.Log("=== VR Setup Status ===");
        
        // Check if XR is enabled
#if ENABLE_VR || ENABLE_AR
        Debug.Log("✓ XR Support is enabled");
#else
        Debug.LogWarning("⚠ XR Support is not enabled");
#endif
        
        // Check VR device
        if (UnityEngine.XR.XRSettings.enabled)
        {
            Debug.Log($"✓ VR Device: {UnityEngine.XR.XRSettings.loadedDeviceName}");
            Debug.Log($"✓ VR Display: {UnityEngine.XR.XRSettings.eyeTextureWidth}x{UnityEngine.XR.XRSettings.eyeTextureHeight}");
        }
        else
        {
            Debug.LogWarning("⚠ VR is not enabled or no VR device detected");
        }
        
        // Check for XR Interaction Toolkit
        // Note: Commenting out XROrigin check due to API changes in Unity 6
        Debug.Log("ℹ XR Origin check skipped - check manually in scene hierarchy");
        
        /*
        var xrOrigin = FindFirstObjectByType<UnityEngine.XR.Interaction.Toolkit.XROrigin>();
        if (xrOrigin == null)
        {
            // Try the new name in Unity 6
            var xrOriginNew = FindFirstObjectByType<UnityEngine.XR.Interaction.Toolkit.Locomotion.XROrigin>();
            if (xrOriginNew != null)
            {
                Debug.Log("✓ XR Origin found in scene");
            }
            else
            {
                Debug.LogWarning("⚠ No XR Origin found - add XR Origin (VR) to scene for VR functionality");
            }
        }
        else
        {
            Debug.Log("✓ XR Origin found in scene");
        }
        */
        
        Debug.Log("=== End VR Setup Status ===");
    }
}
