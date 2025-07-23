using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 50, 0);
    
    void Update()
    {
        // Rotate the cube continuously
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
