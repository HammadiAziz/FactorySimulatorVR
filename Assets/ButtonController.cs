using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Animator machineAnimator; 
    public GameObject warningCube;   
    public AudioSource alarmSound;   
    public GameObject smokeEffect;   
    public GameObject additionalObject; 

    private int pressCount = 0;      

    private void Start()
    {
        
        if (machineAnimator != null)
        {
            machineAnimator.Play("Idle"); 
        }

       
        if (warningCube != null)
        {
            warningCube.SetActive(false);
        }
        if (smokeEffect != null)
        {
            smokeEffect.SetActive(false);
        }
        if (additionalObject != null)
        {
            additionalObject.SetActive(false);
        }
    }

    public void OnButtonPressed()
    {
        pressCount++;

        switch (pressCount)
        {
            case 1:
                StartMachineAnimation();
                break;

            case 2:
                ShowWarning();
                break;

            case 3:
                PlayAlarmAndSmoke();
                break;

            case 4:
                StopMachineAnimation();
                ShowAdditionalObject();
                break;

            default:
                ResetMachine();
                break;
        }
    }

    private void StartMachineAnimation()
    {
        if (machineAnimator != null)
        {
            machineAnimator.SetTrigger("StartMachine"); 
        }
    }

    private void ShowWarning()
    {
        if (warningCube != null)
        {
            warningCube.SetActive(true); 
        }
    }

    private void PlayAlarmAndSmoke()
    {
        if (alarmSound != null)
        {
            alarmSound.Play(); 
        }
        if (smokeEffect != null)
        {
            smokeEffect.SetActive(true); 
        }
    }

    private void StopMachineAnimation()
    {
        if (machineAnimator != null)
        {
            machineAnimator.SetTrigger("StopMachine"); 
        }
    }

    private void ShowAdditionalObject()
    {
        if (additionalObject != null)
        {
            additionalObject.SetActive(true); 
        }
    }

    private void ResetMachine()
    {
        pressCount = 0; 

        if (warningCube != null)
        {
            warningCube.SetActive(false); 
        }
        if (smokeEffect != null)
        {
            smokeEffect.SetActive(false); 
        }
        if (alarmSound != null)
        {
            alarmSound.Stop(); 
        }
        if (additionalObject != null)
        {
            additionalObject.SetActive(false); 
        }

        if (machineAnimator != null)
        {
            machineAnimator.Play("Idle"); 
        }
    }
}
