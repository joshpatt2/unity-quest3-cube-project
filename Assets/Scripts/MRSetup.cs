using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class MRSetup : MonoBehaviour
{
    [Header("Mixed Reality Settings")]
    public bool enablePassthrough = true;
    public Material passthroughMaterial;
    
    [Header("Scene Objects")]
    public GameObject virtualCube;
    public Camera mainCamera;
    
    private void Start()
    {
        SetupMixedReality();
    }
    
    private void SetupMixedReality()
    {
        Debug.Log("Setting up Mixed Reality environment...");
        
        // Enable passthrough if supported
        if (enablePassthrough)
        {
            EnablePassthrough();
        }
        
        // Configure camera for MR
        SetupMRCamera();
        
        // Position virtual content appropriately for MR
        SetupVirtualContent();
    }
    
    private void EnablePassthrough()
    {
        // Set camera to solid color (black/transparent) for passthrough
        if (mainCamera == null)
            mainCamera = Camera.main;
            
        if (mainCamera != null)
        {
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = Color.clear; // Transparent for passthrough
            
            Debug.Log("Passthrough camera settings applied");
        }
        
        // Note: Actual passthrough enablement happens at the XR system level
        // This is handled by the Oculus XR Plugin when passthrough permissions are granted
    }
    
    private void SetupMRCamera()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
            
        if (mainCamera != null)
        {
            // Optimize rendering for MR
            mainCamera.nearClipPlane = 0.01f; // Very close objects
            mainCamera.farClipPlane = 1000f;  // Distant objects
            
            // Enable depth testing for proper occlusion
            mainCamera.depthTextureMode = DepthTextureMode.Depth;
            
            Debug.Log("MR camera configured");
        }
    }
    
    private void SetupVirtualContent()
    {
        if (virtualCube != null)
        {
            // Position the cube in front of the user at a comfortable distance
            virtualCube.transform.position = new Vector3(0, 1.5f, 2f); // 2 meters in front, at eye level
            virtualCube.transform.localScale = Vector3.one * 0.3f; // Smaller scale for MR
            
            // Make sure the cube has appropriate materials for MR
            var renderer = virtualCube.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Use a material that works well in MR (semi-transparent or bright colors work best)
                var material = renderer.material;
                if (material != null)
                {
                    // Make it slightly transparent and emissive for better visibility in MR
                    if (material.HasProperty("_Color"))
                    {
                        Color color = material.color;
                        color.a = 0.8f; // Slightly transparent
                        material.color = color;
                    }
                    
                    // Add some emission to make it stand out
                    if (material.HasProperty("_EmissionColor"))
                    {
                        material.SetColor("_EmissionColor", Color.blue * 0.3f);
                        material.EnableKeyword("_EMISSION");
                    }
                }
            }
            
            Debug.Log($"Virtual cube positioned for MR at: {virtualCube.transform.position}");
        }
    }
    
    private void Update()
    {
        // Optional: Add hand tracking or controller interaction here
        HandleMRInteractions();
    }
    
    private void HandleMRInteractions()
    {
        // Check for XR input (controllers or hands)
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);
        
        foreach (var device in inputDevices)
        {
            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Controller))
            {
                // Handle controller input for MR interactions
                if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
                {
                    if (triggerPressed && virtualCube != null)
                    {
                        // Simple interaction: change cube color when trigger is pressed
                        var renderer = virtualCube.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.8f, 1f);
                        }
                    }
                }
            }
        }
    }
    
    // Public method to toggle passthrough (can be called from UI)
    public void TogglePassthrough()
    {
        enablePassthrough = !enablePassthrough;
        if (enablePassthrough)
        {
            EnablePassthrough();
            Debug.Log("Passthrough enabled");
        }
        else
        {
            // Switch back to VR mode
            if (mainCamera != null)
            {
                mainCamera.clearFlags = CameraClearFlags.Skybox;
                mainCamera.backgroundColor = Color.black;
            }
            Debug.Log("Passthrough disabled - VR mode");
        }
    }
}
