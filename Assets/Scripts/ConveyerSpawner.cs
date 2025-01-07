using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerSpawner : MonoBehaviour
{
    [Header("Plane Settings")]
    public GameObject planePrefab; // Plane prefab to spawn
    public float spawnDelay = 1f; // Delay before spawning a new plane
    public int maxPlanes = 8; // Maximum number of planes to spawn

    private static bool isSpawning = false; // Prevent multiple spawn loops
    private int planeCount = 0; // Count the number of spawned planes

    private void Start()
    {
        // Start plane spawning process if not already started
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnPlanes());
        }
    }

    private IEnumerator SpawnPlanes()
    {
        // Get the parent of the assigned planePrefab
        Transform parentTransform = planePrefab.transform.parent;

        while (planeCount < maxPlanes)
        {
            yield return new WaitForSeconds(spawnDelay);

            // Spawn a new plane at the same position/rotation as the prefab
            GameObject newPlane = Instantiate(planePrefab, planePrefab.transform.position, planePrefab.transform.rotation);

            // Set the parent of the new plane to the parent of the prefab
            if (parentTransform != null)
            {
                newPlane.transform.SetParent(parentTransform, true); // Attach as a child while maintaining world position/rotation
            }

            planeCount++;
            Debug.Log("New plane spawned as a child of the prefab's parent.");

            // Stop spawning after reaching max plane count
            if (planeCount >= maxPlanes)
            {
                Debug.Log("Max plane count reached.");
                break;
            }
        }
    }
}
