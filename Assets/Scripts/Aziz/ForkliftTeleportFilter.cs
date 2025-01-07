using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals;

public class ForkliftTeleportFilter : MonoBehaviour
{
    public bool isInForklift = false; // Track whether the player is in the forklift

    // This method is called to check if teleportation is allowed
    public bool CanTeleport(XRInteractorLineVisual interactor, IXRHoverInteractable interactable)
    {
        // Prevent teleportation if the player is in the forklift
        return !isInForklift;
    }

    // Call this method when entering the forklift
    public void EnterForklift()
    {
        isInForklift = true;
    }

    // Call this method when exiting the forklift
    public void ExitForklift()
    {
        isInForklift = false;
    }
}
