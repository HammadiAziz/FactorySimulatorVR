using UnityEngine;

public class ForkliftInteractionManager : MonoBehaviour
{
    public Transform forklift;          // Reference to the forklift GameObject
    public Transform player;            // Reference to the player's XR Rig or GameObject
    public GameObject playerController; // The player's movement script or Rigidbody
    public GameObject forkliftController; // ForkliftController script

    private bool isInForklift = false;

    public void EnterForklift()
    {
        if (!isInForklift)
        {
            // Parent the player to the forklift
            player.SetParent(forklift);

            // Align the player to the seat position
            player.position = forklift.position; // Adjust based on seat position
            player.rotation = forklift.rotation;

            // Disable player's independent movement
            if (playerController != null)
            {
                playerController.SetActive(false);
            }

            // Enable forklift controls
            forkliftController.SetActive(true);

            isInForklift = true;
        }
    }

    public void ExitForklift()
    {
        if (isInForklift)
        {
            // Unparent the player from the forklift
            player.SetParent(null);

            // Enable player's independent movement
            if (playerController != null)
            {
                playerController.SetActive(true);
            }

            // Disable forklift controls
            forkliftController.SetActive(false);

            isInForklift = false;
        }
    }
}
