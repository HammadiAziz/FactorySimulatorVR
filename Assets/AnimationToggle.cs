using UnityEngine;

public class AnimationToggle : MonoBehaviour
{
    // Assign the GameObject with the AudioSource in the Inspector
    public GameObject audioSourceObject;

    public void ToggleConveyorAnimation()
    {
        // Find all objects with the tag "conveyer"
        GameObject[] conveyors = GameObject.FindGameObjectsWithTag("conveyer");

        foreach (GameObject conveyor in conveyors)
        {
            // Toggle the Animator
            Animator animator = conveyor.GetComponent<Animator>();
            if (animator != null)
            {
                if (animator.speed == 0)
                {
                    animator.speed = 1;
                }
                else
                {
                    animator.speed = 0;
                }
            }
        }

        // Toggle the AudioSource
        if (audioSourceObject != null)
        {
            AudioSource audioSource = audioSourceObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Pause();
                }
                else
                {
                    audioSource.Play();
                }
            }
        }
    }
}
