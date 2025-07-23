using UnityEngine;

public class EnsureCubeSpins : MonoBehaviour
{
    [Header("Cube Settings")]
    public float rotationSpeed = 50f;
    public bool findCubeAutomatically = true;
    
    private GameObject cubeObject;
    private CubeRotator cubeRotator;
    
    void Start()
    {
        // Wait a frame to ensure everything is initialized
        Invoke(nameof(SetupCubeRotation), 0.1f);
    }
    
    void SetupCubeRotation()
    {
        Debug.Log("[EnsureCubeSpins] Setting up cube rotation...");
        
        // Find the cube
        FindCube();
        
        if (cubeObject != null)
        {
            // Ensure the cube has a CubeRotator component
            EnsureRotatorComponent();
            
            // Start the rotation
            StartCubeRotation();
        }
        else
        {
            Debug.LogWarning("[EnsureCubeSpins] No cube found in scene!");
        }
    }
    
    void FindCube()
    {
        if (findCubeAutomatically)
        {
            // Try multiple methods to find the cube
            cubeObject = GameObject.Find("Cube");
            
            if (cubeObject == null)
            {
                cubeObject = GameObject.FindWithTag("Cube");
            }
            
            if (cubeObject == null)
            {
                // Look for any object with "cube" in the name (case insensitive)
                GameObject[] allObjects = FindObjectsOfType<GameObject>();
                foreach (GameObject obj in allObjects)
                {
                    if (obj.name.ToLower().Contains("cube"))
                    {
                        cubeObject = obj;
                        break;
                    }
                }
            }
            
            if (cubeObject == null)
            {
                // Create a cube if none exists
                Debug.Log("[EnsureCubeSpins] No cube found, creating one...");
                CreateCube();
            }
        }
        
        if (cubeObject != null)
        {
            Debug.Log($"[EnsureCubeSpins] Found cube: {cubeObject.name}");
        }
    }
    
    void CreateCube()
    {
        cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeObject.name = "MR Cube";
        
        // Position the cube in front of the user for MR
        cubeObject.transform.position = new Vector3(0, 1.5f, 2f);
        cubeObject.transform.localScale = Vector3.one * 0.3f;
        
        // Make it more visible in MR
        Renderer renderer = cubeObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material;
            material.color = Color.cyan;
            
            // Add some emission for better visibility in MR
            if (material.HasProperty("_EmissionColor"))
            {
                material.SetColor("_EmissionColor", Color.blue * 0.3f);
                material.EnableKeyword("_EMISSION");
            }
        }
        
        Debug.Log("[EnsureCubeSpins] Created new cube for MR experience");
    }
    
    void EnsureRotatorComponent()
    {
        cubeRotator = cubeObject.GetComponent<CubeRotator>();
        
        if (cubeRotator == null)
        {
            Debug.Log("[EnsureCubeSpins] Adding CubeRotator component...");
            cubeRotator = cubeObject.AddComponent<CubeRotator>();
        }
        
        // Configure the rotator (simplified - only rotation speed)
        cubeRotator.rotationSpeed = rotationSpeed;
    }
    
    void StartCubeRotation()
    {
        if (cubeRotator != null)
        {
            Debug.Log("[EnsureCubeSpins] Starting cube rotation...");
            // No need to call StartRotation() - the simplified CubeRotator always rotates
            
            // Verify it's actually rotating
            Invoke(nameof(VerifyRotation), 2f);
        }
    }
    
    void VerifyRotation()
    {
        if (cubeObject != null)
        {
            Vector3 currentRotation = cubeObject.transform.rotation.eulerAngles;
            Debug.Log($"[EnsureCubeSpins] Cube rotation after 2 seconds: {currentRotation}");
            
            if (cubeRotator != null)
            {
                Debug.Log($"[EnsureCubeSpins] CubeRotator enabled: {cubeRotator.enabled}, speed: {cubeRotator.rotationSpeed}");
            }
        }
    }
    
    // Public method to manually trigger setup
    [ContextMenu("Setup Cube Rotation")]
    public void ManualSetup()
    {
        SetupCubeRotation();
    }
}
