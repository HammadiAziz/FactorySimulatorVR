using UnityEngine;

public class LimitGrabRotation : MonoBehaviour
{
    public Transform target; // The object being grabbed
    public float minZRotation = -90f; // Minimum allowed Z-axis rotation
    public float maxZRotation = 90f; // Maximum allowed Z-axis rotation

    private Quaternion initialRotation; // The initial rotation of the object

    void Start()
    {
        // Save the initial rotation of the object
        initialRotation = target.localRotation;
    }

    void Update()
    {
        // Limit the Z-axis rotation
        Vector3 currentEulerAngles = target.localRotation.eulerAngles;

        // Convert Z-angle to range [-180, 180] for clamping
        float zRotation = currentEulerAngles.z;
        if (zRotation > 180f)
        {
            zRotation -= 360f;
        }

        // Clamp Z-axis rotation
        zRotation = Mathf.Clamp(zRotation, minZRotation, maxZRotation);

        // Apply constrained rotation back to the object, maintaining other axes
        target.localRotation = Quaternion.Euler(initialRotation.eulerAngles.x, initialRotation.eulerAngles.y, zRotation);
    }
}
