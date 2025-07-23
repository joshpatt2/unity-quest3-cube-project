using UnityEngine;

public class AppInitializer : MonoBehaviour
{
    [Header("Components")]
    public XRImmersiveInitializer xrInitializer;
    public Quest3PassthroughManager passthroughManager;
    public CubeSetupManager cubeSetupManager;
    
    [Header("Auto Setup")]
    public bool autoSetupOnStart = true;
    public bool enableDebugLogs = true;
    
    private void Start()
    {

            InitializeApp();
  
    }
    
    public void InitializeApp()
    {
        LogDebug("Initializing Quest 3 MR App...");
        
        // Step 1: Find or create necessary components
        SetupComponents();
        
        // Step 2: Initialize XR first (this is critical for immersive experience)
        InitializeXR();
        
        // Step 3: Setup the cube
        InitializeCube();
        
        // Step 4: Configure MR/Passthrough
        InitializePassthrough();
        
        LogDebug("App initialization complete!");
    }
    
    private void SetupComponents()
    {
        // Find XR Initializer or create one
        if (xrInitializer == null)
        {
            xrInitializer = FindObjectOfType<XRImmersiveInitializer>();
            if (xrInitializer == null)
            {
                GameObject xrObj = new GameObject("XR Initializer");
                xrInitializer = xrObj.AddComponent<XRImmersiveInitializer>();
                LogDebug("Created XRImmersiveInitializer");
            }
        }
        
        // Find Passthrough Manager or create one
        if (passthroughManager == null)
        {
            passthroughManager = FindObjectOfType<Quest3PassthroughManager>();
            if (passthroughManager == null)
            {
                GameObject ptObj = new GameObject("Passthrough Manager");
                passthroughManager = ptObj.AddComponent<Quest3PassthroughManager>();
                LogDebug("Created Quest3PassthroughManager");
            }
        }
        
        // Find Cube Setup Manager or create one
        if (cubeSetupManager == null)
        {
            cubeSetupManager = FindObjectOfType<CubeSetupManager>();
            if (cubeSetupManager == null)
            {
                GameObject cubeObj = new GameObject("Cube Setup Manager");
                cubeSetupManager = cubeObj.AddComponent<CubeSetupManager>();
                LogDebug("Created CubeSetupManager");
            }
        }
    }
    
    private void InitializeXR()
    {
        if (xrInitializer != null)
        {
            xrInitializer.initializeXROnStart = true;
            xrInitializer.enablePassthroughOnStart = true;
            LogDebug("XR Initializer configured");
        }
    }
    
    private void InitializeCube()
    {
        if (cubeSetupManager != null)
        {
            // The CubeSetupManager will handle cube creation and rotation setup in its Start method
            LogDebug("Cube setup will be handled by CubeSetupManager");
        }
    }
    
    private void InitializePassthrough()
    {
        if (passthroughManager != null)
        {
            passthroughManager.startWithPassthrough = true;
            LogDebug("Passthrough manager configured to start with passthrough enabled");
        }
    }
    
    private void LogDebug(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[AppInitializer] {message}");
        }
    }
    
    // Public methods for runtime control
    public void RestartApp()
    {
        LogDebug("Restarting app initialization...");
        InitializeApp();
    }
    
    public void ToggleCubeRotation()
    {
        if (cubeSetupManager != null)
        {
            cubeSetupManager.ToggleRotation();
        }
    }
    
    public void ResetCube()
    {
        if (cubeSetupManager != null)
        {
            cubeSetupManager.ResetCube();
        }
    }
    
    public void TogglePassthrough()
    {
        if (passthroughManager != null)
        {
            passthroughManager.TogglePassthrough();
        }
    }
}
