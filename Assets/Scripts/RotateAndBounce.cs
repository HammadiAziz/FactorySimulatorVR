using UnityEngine;

public class RotateAndBounce : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 50f, 0f); // Rotation speed in degrees per second
    public float bounceSpeed = 2f; // Speed of the bounce
    public float bounceHeight = 0.5f; // Height of the bounce

    private Vector3 initialPosition; // Initial position of the object

    void Start()
    {
        // Record the object's starting position
        initialPosition = transform.position;
    }

    void Update()
    {
        // Rotate the object
        RotateObject();

        // Bounce the object
        BounceObject();
    }

    private void RotateObject()
    {
        // Apply rotation based on rotationSpeed
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    private void BounceObject()
    {
        // Calculate the bounce offset using a sine wave
        float bounceOffset = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;

        // Apply the bounce offset to the Y position
        transform.position = new Vector3(initialPosition.x, initialPosition.y + bounceOffset, initialPosition.z);
    }
}
