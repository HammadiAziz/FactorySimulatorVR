using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SteeringWheelController : MonoBehaviour
{
    public float maxSteeringAngle = 90f; // Maximum steering angle in degrees
    private float currentSteeringAngle = 0f; // Current angle of the steering wheel
    private XRBaseInteractor interactor; // The interactor (VR controller) interacting with the wheel
    private Quaternion initialWheelRotation; // Initial rotation of the steering wheel
    private Vector3 grabStartDirection; // Initial direction of the grab

    private void Start()
    {
        // Save the initial rotation of the steering wheel
        initialWheelRotation = transform.localRotation;
    }

    public void OnGrabStarted(XRBaseInteractor newInteractor)
    {
        // Save the interactor grabbing the wheel
        interactor = newInteractor;

        // Save the initial grab direction relative to the steering wheel
        grabStartDirection = transform.InverseTransformPoint(interactor.transform.position).normalized;
    }

    public void OnGrabEnded()
    {
        // Clear the interactor reference
        interactor = null;
    }

    private void Update()
    {
        if (interactor != null)
        {
            RotateSteeringWheel();
        }
    }

    private void RotateSteeringWheel()
    {
        // Get the current grab direction relative to the wheel
        Vector3 currentGrabDirection = transform.InverseTransformPoint(interactor.transform.position).normalized;

        // Calculate the angle difference between the initial and current grab directions
        float angleDifference = Vector3.SignedAngle(grabStartDirection, currentGrabDirection, Vector3.forward); // Adjust axis if needed

        // Update the steering angle, clamping it to the maximum allowed range
        currentSteeringAngle = Mathf.Clamp(currentSteeringAngle + angleDifference, -maxSteeringAngle, maxSteeringAngle);

        // Apply the rotation to the steering wheel
        transform.localRotation = initialWheelRotation * Quaternion.Euler(0, 0, currentSteeringAngle); // Change Y-axis to the desired rotation axis
    }

    public float GetSteeringAngle()
    {
        // Return the current steering angle
        return currentSteeringAngle;
    }
}
