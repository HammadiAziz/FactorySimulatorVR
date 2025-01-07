using Unity.Mathematics;
using UnityEngine;

public class PoliceEffects : MonoBehaviour
{
    public Light policeLight; 
    public AudioSource sirenSound;
    public GameObject ui;
    private bool isActive = false;

    public void StartEffects()
    {
        if (!isActive)
        {
            isActive = true;

            if (policeLight != null)
            {
                policeLight.enabled = true;
            }

            if (sirenSound != null)
            {
                sirenSound.Play();
            }
            if (ui != null)
            {
                ui.SetActive(true);
            }

            Invoke(nameof(StopEffects), 5f);
        }
    }

    public void StopEffects()
    {
        isActive = false;

        if (policeLight != null)
        {
            policeLight.enabled = false;
        }

        if (sirenSound != null)
        {
            sirenSound.Stop();
        }
       
    }
}
