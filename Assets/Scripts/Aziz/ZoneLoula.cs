using UnityEngine;

public class ZoneLoula:MonoBehaviour
{
   
    public Material newMaterial; // Assign this in the Inspector
    public GameObject targetObject; // Assign the target GameObject in the Inspector

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("Target GameObject is not assigned!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("pallet"))
        {
            if (newMaterial != null && targetObject != null)
            {
                ChangeMaterial(targetObject, newMaterial);
            }
            else
            {
                Debug.LogWarning("Target GameObject or new material is not assigned!");
            }
        }
    }

    void ChangeMaterial(GameObject obj, Material material)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = material;
            }
            renderer.materials = materials;
        }
    }
}


    
