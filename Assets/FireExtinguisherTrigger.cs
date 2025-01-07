using UnityEngine;
using UnityEngine;

public class FireExtinguisherTrigger : MonoBehaviour
{
    public ParticleSystem sprayEffect; // Drag your particle system here in the Inspector
    public AudioSource spraySound; // Drag your AudioSource here in the Inspector
    public float pressure = 100f; // Maximum pressure value
    public float pressureDecrement = 1f; // How much pressure decreases per second

    private bool isSpraying = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the VR controller interacts
        if (other.CompareTag("PlayerHand"))
        {
            StartSpraying();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            StopSpraying();
        }
    }

    private void Update()
    {
        if (isSpraying && pressure > 0)
        {
            pressure -= pressureDecrement * Time.deltaTime;
            if (pressure <= 0)
            {
                StopSpraying();
            }
        }
    }

    private void StartSpraying()
    {
        if (!isSpraying && pressure > 0)
        {
            isSpraying = true;
            sprayEffect.Play();
            spraySound.Play();
        }
    }

    private void StopSpraying()
    {
        isSpraying = false;
        sprayEffect.Stop();
        spraySound.Stop();
    }
}

