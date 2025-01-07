using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxCollisionChecker : MonoBehaviour
{
    public string boxTag = "Box"; // Tag for the boxes
    public GameObject dropMessageUI; // Reference to the UI panel for drop notification
    public GameObject restartButton; // Reference to the restart button (or additional UI)

    private void Start()
    {
        // Ensure the UI is hidden at the start
        if (dropMessageUI != null)
        {
            dropMessageUI.SetActive(false); // Hide the UI initially
        }
        else
        {
            Debug.LogWarning("Drop Message UI is not assigned in the Inspector!");
        }

        // Ensure the restart button is hidden at the start
        if (restartButton != null)
        {
            restartButton.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Restart Button UI is not assigned in the Inspector!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the "Box" tag
        if (other.CompareTag(boxTag))
        {
            BoxDropped(other.gameObject); // Handle the dropped box
        }
    }

    private void BoxDropped(GameObject box)
    {
        Debug.Log($"Box {box.name} has entered the trigger and is considered dropped!");

        // Show the drop message UI
        if (dropMessageUI != null)
        {
            dropMessageUI.SetActive(true);
        }

        // Show the restart button
        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }

        // Freeze the game
        Time.timeScale = 0; // Freeze all game activity

        // Optionally disable the box's movement
        Rigidbody rb = box.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Stop the box from moving
        }
    }

    // Method to restart the game
    public void RestartGame()
    {
        Time.timeScale = 1; // Unfreeze the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
