

using UnityEngine.UI;
using UnityEngine;

public class MachineController : MonoBehaviour
{
    public Animator machineAnimator; // Reference to the machine's Animator
    public GameObject overheatingUI; // Reference to the overheating UI
    public GameObject overheatingSmoke; // Reference to the overheating smoke GameObject
    public AudioSource alarmSound;   // Reference to the alarm AudioSource

    private int pressCount = 0; // Counter for button presses

    void Start()
    {
        // Ensure UI and smoke are disabled initially
        if (overheatingUI != null)
        {
            overheatingUI.SetActive(false); // Disable overheating UI initially
        }
        if (overheatingSmoke != null)
        {
            overheatingSmoke.SetActive(false); // Disable smoke GameObject initially
        }
    }

    public void OnButtonPress()
    {
        pressCount++;

        switch (pressCount)
        {
            case 1:
                StartMachineAnimation();
                break;

            case 2:
                ShowOverheatingWarning();
                break;

            case 3:
                PlayAlarmSound();
                break;

            case 4:
                ShowOverheatingSmoke();
                break;

            default:
                ResetMachineState();
                break;
        }
    }

    void StartMachineAnimation()
    {
        if (machineAnimator != null)
        {
            machineAnimator.SetTrigger("Start"); // Start machine animation
        }
    }

    void ShowOverheatingWarning()
    {
        if (overheatingUI != null)
        {
            overheatingUI.SetActive(true); // Show the overheating warning
        }
    }

    void PlayAlarmSound()
    {
        if (alarmSound != null)
        {
            alarmSound.Play(); // Play the alarm sound
        }
    }

    void ShowOverheatingSmoke()
    {
        if (overheatingSmoke != null)
        {
            overheatingSmoke.SetActive(true); // Enable the overheating smoke GameObject
        }
    }

    void ResetMachineState()
    {
        // Reset the machine to the initial state
        pressCount = 0;

        if (overheatingUI != null)
        {
            overheatingUI.SetActive(false); // Disable overheating UI
        }
        if (overheatingSmoke != null)
        {
            overheatingSmoke.SetActive(false); // Disable overheating smoke GameObject
        }
        if (alarmSound != null)
        {
            alarmSound.Stop(); // Stop the alarm sound
        }
    }
}
