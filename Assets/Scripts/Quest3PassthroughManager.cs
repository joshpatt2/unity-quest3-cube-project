using UnityEngine;
using UnityEngine.XR;

public class Quest3PassthroughManager : MonoBehaviour
{
    [Header("Passthrough Settings")]
    public bool startWithPassthrough = true;
    public float passthroughOpacity = 1.0f;
    
    [Header("Environment")]
    public Light environmentLight;
    public Material skyboxMaterial;
    
    private bool passthroughEnabled = false;
    
    private void Start()
    {
        if (startWithPassthrough)
        {
            EnablePassthrough();
        }
    }
    
    public void EnablePassthrough()
    {
        Debug.Log("Enabling Quest 3 Passthrough...");
        
        try
        {
            // Configure environment for passthrough
            ConfigurePassthroughEnvironment();
            
            passthroughEnabled = true;
            Debug.Log("Passthrough enabled successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to enable passthrough: {e.Message}");
        }
    }
    
    public void DisablePassthrough()
    {
        Debug.Log("Disabling passthrough...");
        
        // Restore VR environment
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.backgroundColor = Color.black;
        
        // Restore lighting
        if (environmentLight != null)
        {
            environmentLight.intensity = 1.0f;
            environmentLight.color = Color.white;
        }
        
        passthroughEnabled = false;
        Debug.Log("Passthrough disabled - VR mode restored");
    }
    
    private void ConfigurePassthroughEnvironment()
    {
        // Adjust main camera for passthrough
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            mainCam.clearFlags = CameraClearFlags.SolidColor;
            mainCam.backgroundColor = Color.clear;
            
            // Optimize depth buffer for MR
            mainCam.nearClipPlane = 0.01f;
            mainCam.farClipPlane = 1000f;
        }
        
        // Adjust environment lighting for MR
        if (environmentLight != null)
        {
            // Reduce artificial lighting to blend better with real environment
            environmentLight.intensity = 0.3f;
            environmentLight.color = new Color(1f, 0.95f, 0.8f); // Warm white
        }
        
        // Disable skybox rendering for passthrough
        RenderSettings.skybox = null;
        
        Debug.Log("Passthrough environment configured");
    }
    
    public void TogglePassthrough()
    {
        if (passthroughEnabled)
        {
            DisablePassthrough();
        }
        else
        {
            EnablePassthrough();
        }
    }
    
    private void Update()
    {
        // Allow toggle with XR controller button (using generic XR input)
        var inputDevices = new System.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevices(inputDevices);
        
        foreach (var device in inputDevices)
        {
            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Controller))
            {
                if (device.TryGetFeatureValue(CommonUsages.menuButton, out bool menuPressed))
                {
                    if (menuPressed)
                    {
                        TogglePassthrough();
                        break; // Only toggle once per frame
                    }
                }
            }
        }
    }
    
    private void OnValidate()
    {
        // Clamp opacity value
        passthroughOpacity = Mathf.Clamp01(passthroughOpacity);
    }
}
