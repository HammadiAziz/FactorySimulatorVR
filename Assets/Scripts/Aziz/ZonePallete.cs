using UnityEngine;

public class ZonePallete : MonoBehaviour
{
    public string paletteTag = "pallet"; // Tag for the palette
    public Material firstMaterial; // Material to apply on initial collision
    public Material secondMaterial; // Material to apply later externally
    private bool isFirstMaterialApplied = false; // Tracks if the first material was applied

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(paletteTag)) return; // Ensure the collider is the pallet

        Debug.Log($"Something entered the trigger: {other.gameObject.name}");

        // Apply the first material if it hasn't been applied yet
        if (!isFirstMaterialApplied)
        {
            ApplyMaterial(firstMaterial);
            isFirstMaterialApplied = true;
            Debug.Log("First material applied.");
        }
        else
        {
            Debug.Log("First material already applied. Ignoring additional triggers.");
        }
    }

    // Public method to apply the second material
    public void ApplyMaterialChange()
    {
        if (isFirstMaterialApplied)
        {
            ApplyMaterial(secondMaterial);
            Debug.Log("Second material applied.");
        }
        else
        {
            Debug.LogWarning("First material has not been applied yet. Cannot apply second material.");
        }
    }

    // Helper method to apply a material to all child objects
    private void ApplyMaterial(Material material)
    {
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        if (childRenderers.Length > 0)
        {
            foreach (Renderer renderer in childRenderers)
            {
                renderer.material = material;
            }
            Debug.Log($"Material {material.name} applied to all children.");
        }
        else
        {
            Debug.LogError("No Renderer components found in children!");
        }
    }
}
