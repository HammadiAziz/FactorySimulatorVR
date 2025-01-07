using System.Collections.Generic;
using UnityEngine;

public class AttachDetachObject : MonoBehaviour
{
    public List<Transform> attachPoints; // List of attach points
    public List<Transform> detachPoints; // List of detach points
    public float detectionRange = 0.4f;  // Range for detecting objects to attach
    public LayerMask interactableLayer;  // Layer for interactable objects
    public float attachCooldown = 2.0f;  // Cooldown time after detachment

    private Dictionary<GameObject, Transform> attachedObjects = new Dictionary<GameObject, Transform>(); // Tracks attached objects
    private Dictionary<GameObject, Transform> detachedObjects = new Dictionary<GameObject, Transform>(); // Tracks objects assigned to detach points
    private HashSet<GameObject> recentlyDetached = new HashSet<GameObject>(); // Tracks recently detached objects

    void Update()
    {
        foreach (var attachPoint in attachPoints)
        {
            // Check if this attach point is free
            if (!attachedObjects.ContainsValue(attachPoint))
            {
                // Detect nearby objects
                Collider[] nearbyObjects = Physics.OverlapSphere(attachPoint.position, detectionRange, interactableLayer);

                foreach (var collider in nearbyObjects)
                {
                    GameObject obj = collider.gameObject;

                    // Only attach objects with the "milkrubber" tag and not recently detached
                    if (obj.CompareTag("milkrubber") && !recentlyDetached.Contains(obj) && !attachedObjects.ContainsKey(obj))
                    {
                        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable = obj.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
                        if (interactable != null && !interactable.isSelected) // Ensure not currently grabbed
                        {
                            Debug.Log($"Attaching object: {obj.name} to {attachPoint.name}");
                            AttachObject(obj, attachPoint);
                            break;
                        }
                    }
                }
            }
        }

        // Update attached objects' positions
        foreach (var kvp in attachedObjects)
        {
            GameObject obj = kvp.Key;
            Transform attachPoint = kvp.Value;

            if (obj != null)
            {
                // Keep the object aligned with the attach point
                obj.transform.position = attachPoint.position;
                obj.transform.rotation = attachPoint.rotation;
            }
        }
    }

    private void AttachObject(GameObject obj, Transform attachPoint)
    {
        if (attachedObjects.ContainsKey(obj))
        {
            Debug.LogWarning($"Object {obj.name} is already attached to another point.");
            return;
        }

        // Attach the object
        attachedObjects[obj] = attachPoint;

        // Disable Rigidbody physics
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Ensure it remains interactable for the VR player
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable = obj.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (interactable != null)
        {
            interactable.selectExited.AddListener(_ => DetachObject(obj));
        }
    }

    private void DetachObject(GameObject obj)
    {
        if (attachedObjects.ContainsKey(obj))
        {
            Debug.Log($"Detaching object: {obj.name}");

            // Remove from the dictionary
            attachedObjects.Remove(obj);

            // Find an available detach point
            Transform availableDetachPoint = GetAvailableDetachPoint();
            if (availableDetachPoint != null)
            {
                // Move to the available detach point
                detachedObjects[obj] = availableDetachPoint;
                obj.transform.position = availableDetachPoint.position;
                obj.transform.rotation = availableDetachPoint.rotation;
                Debug.Log($"Object {obj.name} moved to detach point: {availableDetachPoint.name}");
            }
            else
            {
                Debug.LogWarning("No available detach points!");
            }

            // Enable Rigidbody physics
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Add to recently detached set
            recentlyDetached.Add(obj);
            Invoke("RemoveFromRecentlyDetached", attachCooldown);

            // Remove any event listeners
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable = obj.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            if (interactable != null)
            {
                interactable.selectExited.RemoveListener(_ => DetachObject(obj));
            }
        }
    }

    private Transform GetAvailableDetachPoint()
    {
        foreach (var point in detachPoints)
        {
            // Check if this point is not occupied
            if (!detachedObjects.ContainsValue(point))
            {
                return point;
            }
        }
        return null; // No available points
    }

    private void RemoveFromRecentlyDetached()
    {
        List<GameObject> detachedObjectsToRemove = new List<GameObject>(recentlyDetached);
        foreach (var obj in detachedObjectsToRemove)
        {
            recentlyDetached.Remove(obj);
            Debug.Log($"Cooldown expired for object: {obj.name}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DetachZone"))
        {
            Debug.Log("Cube entered DetachZone. Detaching all objects.");

            List<GameObject> objectsToDetach = new List<GameObject>(attachedObjects.Keys);
            foreach (var obj in objectsToDetach)
            {
                DetachObject(obj);
            }
        }
    }
}
