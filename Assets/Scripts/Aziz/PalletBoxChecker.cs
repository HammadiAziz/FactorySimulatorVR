using UnityEngine;

public class PalletBoxChecker : MonoBehaviour
{
    public string boxTag = "Box"; // Tag for the boxes
    public GameObject targetObjectToActivate; // The object to activate when condition is met
    public GameObject[] objectsToDeactivate; // Array of objects to deactivate
    private int boxCount = 0; // Tracks the number of boxes in the trigger

    private void Start()
    {
        // Ensure the target object is initially inactive
        if (targetObjectToActivate != null)
        {
            targetObjectToActivate.SetActive(false);
        }
        else
        {
            Debug.LogError("Target object to activate is not assigned!");
        }

        // Log a warning if there are no objects to deactivate
        if (objectsToDeactivate.Length == 0)
        {
            Debug.LogWarning("No objects assigned to deactivate!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the correct tag
        if (other.CompareTag(boxTag))
        {
            boxCount++;
            Debug.Log($"Box entered. Current count: {boxCount}");

            // Activate the target object if 6 boxes are in place
            if (boxCount == 6)
            {
                if (targetObjectToActivate != null)
                {
                    targetObjectToActivate.SetActive(true);
                    Debug.Log("All 6 boxes placed. Target object activated!");
                }

                // Deactivate the specified objects
                foreach (GameObject obj in objectsToDeactivate)
                {
                    if (obj != null)
                    {
                        obj.SetActive(false);
                        Debug.Log($"Deactivated object: {obj.name}");
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger has the correct tag
        if (other.CompareTag(boxTag))
        {
            boxCount = Mathf.Max(0, boxCount - 1); // Prevent negative count
            Debug.Log($"Box removed. Current count: {boxCount}");

            // Deactivate the target object if boxes fall below 6
            if (boxCount < 6)
            {
                if (targetObjectToActivate != null)
                {
                    targetObjectToActivate.SetActive(false);
                    Debug.Log("Not enough boxes. Target object deactivated.");
                }

                // Reactivate the objects that were deactivated
                foreach (GameObject obj in objectsToDeactivate)
                {
                    if (obj != null)
                    {
                        obj.SetActive(true);
                        Debug.Log($"Reactivated object: {obj.name}");
                    }
                }
            }
        }
    }
}
