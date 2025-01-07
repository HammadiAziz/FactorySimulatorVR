using UnityEngine;
using UnityEngine.XR;

public class ForkliftControllerVR : MonoBehaviour
{
    public Transform steeringWheel; // Reference to the steering wheel controller
    public Transform frontLeftWheel; // Front-left wheel reference
    public Transform frontRightWheel; // Front-right wheel reference
    public Transform lifter; // Reference to the front lifter object
    public float turnSpeed = 200f;
    public float moveSpeed = 5f; // Speed for forward/backward movement
    public float liftSpeed = 2f; // Speed for lifting
    public float maxHeight = 5f; // Maximum height for the lifter
    private float initialHeight; // Initial height of the lifter (used as minimum height)
    public XRNode inputSourceright;
    public XRNode inputSourceleft;
    private Vector2 inputAxis; // Input axis from the controller
    private bool isLiftingUp = false; // Whether the lifter is moving up
    private bool isLiftingDown = false; // Whether the lifter is moving down
    public float maxSteeringAngle = 90f;
    private bool isMovingForward = false; // Whether the forklift is moving forward
    private bool isMovingBackward = false; // Whether the forklift is moving backward
    private bool moveForward = false; // Whether the forklift should move forward
    private bool moveBackward = false; //
   

    void Start()
    {
        // Record the initial height of the lifter
        initialHeight = lifter.localPosition.y;
    }

    void Update()
    {
        GetButtonInput();
        RotateForklift();
        MoveLifter();
       





    }



    private void GetButtonInput()
    {
        // Get the InputDevice for the specified controller (inputSource)
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSourceright);

        if (!device.isValid)
        {
            Debug.LogError("Input Device not valid for the specified XRNode!");
            return;
        }

        // Check for Button B (Primary Button)
        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool isBPressed) && isBPressed)
        {
            // Move forward
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            Debug.Log("Moving Forward");
        }

        // Check for Button A (Secondary Button)
        if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool isAPressed) && isAPressed)
        {
            // Move backward
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
            Debug.Log("Moving Backward");
        }
    }
    private float lastSteeringWheelAngle = 0f; // Stores the steering wheel angle from the last frame

    private void RotateForklift()
    {
        // Get joystick input (assumes XR input setup)
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSourceleft);
        if (!device.isValid)
        {
            Debug.LogError("Input Device not valid for the specified XRNode!");
            return;
        }

        // Read the primary 2D axis (joystick)
        Vector2 joystickInput;
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out joystickInput))
        {
            // Calculate rotation based on joystick X-axis input
            float joystickTurn = joystickInput.x;

            // Rotate the forklift proportionally to joystick input
            float rotationAmount = joystickTurn * turnSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up * rotationAmount);

            // Animate the steering wheel to match joystick input
            if (steeringWheel != null)
            {
                float targetSteeringAngle = joystickTurn * maxSteeringAngle;
                float smoothSteeringAngle = Mathf.Lerp(lastSteeringWheelAngle, targetSteeringAngle, Time.deltaTime * 10f);
                steeringWheel.localRotation = Quaternion.Euler(0, 0, -smoothSteeringAngle);

                // Update the last steering wheel angle
                lastSteeringWheelAngle = smoothSteeringAngle;
            }

            // Debugging: Log values
            Debug.Log($"Joystick Input: {joystickInput.x}, Rotation Amount: {rotationAmount}");
        }
        else
        {
            Debug.LogWarning("Unable to read joystick input.");
        }
    }


    private void UpdateSteeringWheelPosition()
    {
        if (steeringWheel == null)
        {
            Debug.LogError("Steering wheel reference is missing!");
            return;
        }

        // Reset the steering wheel's position relative to the forklift
        steeringWheel.localPosition = Vector3.zero; // Adjust if the wheel has an offset
        steeringWheel.localRotation = Quaternion.identity; // Reset rotation if needed
    }





    private void MoveLifter()
    {
        Vector3 newPosition = lifter.localPosition;

        if (isLiftingUp)
        {
            // Move the lifter up, clamped to the max height
            newPosition.y += liftSpeed * Time.deltaTime;
        }
        else if (isLiftingDown)
        {
            // Move the lifter down, clamped to the initial height
            newPosition.y -= liftSpeed * Time.deltaTime;
        }

        // Clamp the lifter's position between the initial height and the max height
        newPosition.y = Mathf.Clamp(newPosition.y, initialHeight, maxHeight);
        lifter.localPosition = newPosition;
    }

    public void StartLiftingUp()
    {
        isLiftingUp = true;
    }

    public void StopLiftingUp()
    {
        isLiftingUp = false;
    }

    public void StartLiftingDown()
    {
        isLiftingDown = true;
    }

    public void StopLiftingDown()
    {
        isLiftingDown = false;
    }
}