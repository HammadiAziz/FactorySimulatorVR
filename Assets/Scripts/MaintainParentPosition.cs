using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StickWithParent : MonoBehaviour
{
    private Vector3 localPositionOffset; // The object's local position relative to its parent
    private Transform parentTransform; // Reference to the parent transform
    private XRGrabInteractable grabInteractable; // Reference to XR Grab Interactable component

    private void Start()
    {
        // Store the parent transform
        parentTransform = transform.parent;

        // Get the XR Grab Interactable component
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (parentTransform != null)
        {
            // Store the initial offset
            localPositionOffset = transform.localPosition;
        }
        else
        {
            Debug.LogError("Parent transform is missing!");
        }
    }

    private void LateUpdate()
    {
        if (parentTransform == null)
        {
            Debug.LogError("Parent transform is null!");
            return;
        }

        // Update only the position to match the parent's movement
        transform.position = parentTransform.TransformPoint(localPositionOffset);

        // If grabbed, adjust the local position offset dynamically
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            localPositionOffset = parentTransform.InverseTransformPoint(transform.position);
        }

        // Leave the rotation untouched to allow free rotation
    }
}
