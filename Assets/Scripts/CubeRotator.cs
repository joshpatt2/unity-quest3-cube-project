using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 rotationSpeed = new Vector3(0, 50, 0);
    public bool rotateX = false;
    public bool rotateY = true;
    public bool rotateZ = false;
    
    [Header("Debug")]
    public bool showDebugLogs = false;
    
    private bool isRotating = true;
    
    void Start()
    {
        if (showDebugLogs)
        {
            Debug.Log($"CubeRotator started on {gameObject.name}. Rotation speed: {rotationSpeed}");
        }
    }
    
    void Update()
    {
        if (isRotating)
        {
            // Create rotation vector based on enabled axes
            Vector3 currentRotation = Vector3.zero;
            
            if (rotateX) currentRotation.x = rotationSpeed.x;
            if (rotateY) currentRotation.y = rotationSpeed.y;
            if (rotateZ) currentRotation.z = rotationSpeed.z;
            
            // Rotate the cube continuously
            transform.Rotate(currentRotation * Time.deltaTime);
            
            if (showDebugLogs && Time.frameCount % 120 == 0) // Log every 2 seconds at 60fps
            {
                Debug.Log($"Cube rotating: {transform.rotation.eulerAngles}");
            }
        }
    }
    
    // Public methods for external control
    public void StartRotation()
    {
        isRotating = true;
        if (showDebugLogs) Debug.Log("Cube rotation started");
    }
    
    public void StopRotation()
    {
        isRotating = false;
        if (showDebugLogs) Debug.Log("Cube rotation stopped");
    }
    
    public void ToggleRotation()
    {
        isRotating = !isRotating;
        if (showDebugLogs) Debug.Log($"Cube rotation {(isRotating ? "enabled" : "disabled")}");
    }
    
    public void SetRotationSpeed(Vector3 newSpeed)
    {
        rotationSpeed = newSpeed;
        if (showDebugLogs) Debug.Log($"Rotation speed changed to: {rotationSpeed}");
    }
    
    public void SetRotationSpeed(float ySpeed)
    {
        rotationSpeed = new Vector3(0, ySpeed, 0);
        if (showDebugLogs) Debug.Log($"Y rotation speed changed to: {ySpeed}");
    }
}
