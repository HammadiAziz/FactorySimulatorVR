using UnityEngine;

public class RotatingLight : MonoBehaviour
{
    public float rotationSpeed = 120f; // Speed of rotation

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
