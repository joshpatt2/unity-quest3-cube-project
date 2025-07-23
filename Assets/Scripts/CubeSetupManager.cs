using UnityEngine;

public class CubeSetupManager : MonoBehaviour
{
    [Header("Cube Configuration")]
    public float rotationSpeed = 50f;
    public Vector3 cubePosition = new Vector3(0, 1.5f, 2f);
    public Vector3 cubeScale = new Vector3(0.3f, 0.3f, 0.3f);
    
    [Header("Visual Enhancement")]
    public bool makeCubeEmissive = true;
    public Color cubeColor = Color.cyan;
    public Color emissionColor = Color.blue;
    
    private GameObject cube;
    private CubeRotator rotatorScript;
    
    private void Start()
    {
        SetupCube();
    }
    
    private void SetupCube()
    {
        Debug.Log("[CubeSetupManager] Starting cube setup...");
        
        // Find or create the cube
        cube = GameObject.Find("Cube");
        
        if (cube == null)
        {
            Debug.Log("Cube not found, creating a new one...");
            CreateNewCube();
        }
        else
        {
            Debug.Log("Found existing cube, configuring it...");
            ConfigureExistingCube();
        }
        
        // Ensure the cube has a rotator script
        AddRotationScript();
        
        // Configure for MR visibility
        ConfigureForMR();
        
        Debug.Log($"[CubeSetupManager] Cube setup complete! Position: {cube.transform.position}, Scale: {cube.transform.localScale}");
        
        // Add a delayed verification to check if rotation is actually working
        Invoke(nameof(VerifyRotation), 2f);
    }
    
    private void CreateNewCube()
    {
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "Cube";
        
        // Position the cube for MR
        cube.transform.position = cubePosition;
        cube.transform.localScale = cubeScale;
    }
    
    private void ConfigureExistingCube()
    {
        // Position the cube appropriately for MR/VR
        cube.transform.position = cubePosition;
        cube.transform.localScale = cubeScale;
    }
    
    private void AddRotationScript()
    {
        // Check if CubeRotator is already attached
        rotatorScript = cube.GetComponent<CubeRotator>();
        
        if (rotatorScript == null)
        {
            Debug.Log("Adding CubeRotator script to cube...");
            rotatorScript = cube.AddComponent<CubeRotator>();
        }
        
        // Configure rotation speed (simplified - only Y-axis rotation)
        rotatorScript.rotationSpeed = rotationSpeed;
        
        Debug.Log($"Cube rotation configured! Speed: {rotationSpeed} degrees/second on Y-axis");
    }
    
    private void ConfigureForMR()
    {
        // Get the cube's renderer
        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        
        if (cubeRenderer != null)
        {
            // Create or modify material for better MR visibility
            Material cubeMaterial = cubeRenderer.material;
            
            // Set base color
            cubeMaterial.color = cubeColor;
            
            if (makeCubeEmissive)
            {
                // Make the cube emissive so it stands out in MR
                if (cubeMaterial.HasProperty("_EmissionColor"))
                {
                    cubeMaterial.SetColor("_EmissionColor", emissionColor * 0.5f);
                    cubeMaterial.EnableKeyword("_EMISSION");
                }
                
                // Enable HDR rendering for emissive materials
                if (cubeMaterial.HasProperty("_HDR"))
                {
                    cubeMaterial.SetFloat("_HDR", 1f);
                }
            }
            
            // Make it slightly transparent for better blending
            if (cubeMaterial.HasProperty("_Mode"))
            {
                cubeMaterial.SetFloat("_Mode", 3); // Transparent mode
                cubeMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                cubeMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                cubeMaterial.SetInt("_ZWrite", 0);
                cubeMaterial.DisableKeyword("_ALPHATEST_ON");
                cubeMaterial.EnableKeyword("_ALPHABLEND_ON");
                cubeMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                cubeMaterial.renderQueue = 3000;
            }
            
            Debug.Log("Cube material configured for MR visibility");
        }
        
        // Add a light source to the cube for better visibility
        AddCubeLight();
    }
    
    private void AddCubeLight()
    {
        // Check if there's already a light attached
        Light cubeLight = cube.GetComponent<Light>();
        
        if (cubeLight == null)
        {
            cubeLight = cube.AddComponent<Light>();
        }
        
        // Configure the light
        cubeLight.type = LightType.Point;
        cubeLight.color = emissionColor;
        cubeLight.intensity = 2f;
        cubeLight.range = 5f;
        cubeLight.shadows = LightShadows.Soft;
        
        Debug.Log("Point light added to cube for enhanced MR visibility");
    }
    
    // Public method to reset cube if needed
    public void ResetCube()
    {
        if (cube != null)
        {
            cube.transform.position = cubePosition;
            cube.transform.rotation = Quaternion.identity;
            cube.transform.localScale = cubeScale;
            Debug.Log("Cube position and rotation reset");
        }
    }
    
    // Public method to change rotation speed
    public void SetRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
        if (rotatorScript != null)
        {
            rotatorScript.rotationSpeed = newSpeed;
            Debug.Log($"Rotation speed changed to {rotationSpeed}");
        }
    }
    
    // Public method to toggle rotation
    public void ToggleRotation()
    {
        if (rotatorScript != null)
        {
            rotatorScript.enabled = !rotatorScript.enabled;
            Debug.Log($"Cube rotation {(rotatorScript.enabled ? "enabled" : "disabled")}");
        }
    }
    
    // Verification method to check if rotation is working
    private void VerifyRotation()
    {
        if (cube != null && rotatorScript != null)
        {
            Vector3 currentRotation = cube.transform.rotation.eulerAngles;
            Debug.Log($"[CubeSetupManager] Verification - Cube rotation after 2s: {currentRotation}");
            Debug.Log($"[CubeSetupManager] CubeRotator state - enabled: {rotatorScript.enabled}, speed: {rotatorScript.rotationSpeed}");
        }
    }
}
