using UnityEngine;

public class ObjectAppearanceController : MonoBehaviour
{
    [Header("Object References")]
    [Tooltip("The object that triggers the appearance of the second object.")]
    public GameObject triggerObject;

    [Tooltip("The object that will appear when the trigger object appears.")]
    public GameObject controlledObject;

    private bool wasTriggerObjectActive;

    void Start()
    {
        // Ensure the controlledObject is initially inactive
        if (controlledObject != null)
        {
            controlledObject.SetActive(false);
        }

        // Record the initial state of the triggerObject
        wasTriggerObjectActive = triggerObject != null && triggerObject.activeSelf;
    }

    void Update()
    {
        // Ensure the references are set
        if (triggerObject == null || controlledObject == null)
        {
            Debug.LogError("TriggerObject and ControlledObject must be assigned in the Inspector.");
            return;
        }

        // Check if the triggerObject has become active
        if (!wasTriggerObjectActive && triggerObject.activeSelf)
        {
            controlledObject.SetActive(true);
        }

        // Update the recorded state of the triggerObject
        wasTriggerObjectActive = triggerObject.activeSelf;
    }
}
