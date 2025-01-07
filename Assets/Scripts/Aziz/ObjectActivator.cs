using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    public string paletteTag = "pallet"; // Tag for the palette
    public GameObject targetObject;
    public GameObject objectToActivate; // The object with the ZonePallete script

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(paletteTag)) return; // Ensure the collider is the pallet

        Debug.Log($"Pallet collided with {gameObject.name}");

        // Call the ApplyMaterialChange method on the target object
        ZonePallete zonePallete = targetObject.GetComponent<ZonePallete>();
        if (zonePallete != null)
        {
            zonePallete.ApplyMaterialChange();
            Debug.Log("ApplyMaterialChange triggered on the target object.");
        }
        else
        {
            Debug.LogWarning("No ZonePallete script found on the target object.");
        }
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            Debug.Log($"GameObject {objectToActivate.name} has been activated.");
        }
        else
        {
            Debug.LogWarning("No GameObject assigned to activate.");
        }

        // Mark the material as changed to prevent further changes
       
    }
}

