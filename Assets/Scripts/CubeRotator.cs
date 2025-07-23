using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 50f;
    
    void Update()
    {
        // Always rotate around Y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
