using UnityEngine;

public class ZoneAnimation : MonoBehaviour
{
    public float bounceHeight = 2f; // Height of the bounce
    public float bounceSpeed = 2f; // Speed of the bounce
    private Vector3 initialPosition; // Initial position of the object

    private void Start()
    {
        // Record the initial position of the object
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Calculate the bounce effect using a sine wave
        float newY = initialPosition.y + Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed)) * bounceHeight;

        // Ensure the object doesn't go below the initial position
        newY = Mathf.Max(newY, initialPosition.y);

        // Update the object's position
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}
