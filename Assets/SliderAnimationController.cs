using UnityEngine;
using UnityEngine.UI;

public class SliderAnimationController : MonoBehaviour
{
    [Header("References")]
    public Slider speedSlider;              
    public Animator targetAnimator;         
    public GameObject specialObject;        
    [Header("Animation Parameters")]
    public string animationSpeedParam = "Speed";  
    private bool isGameObjectActive = false;

    void Start()
    {
        
        if (specialObject != null)
            specialObject.SetActive(false);
        speedSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        
        if (targetAnimator != null)
        {
            targetAnimator.SetFloat(animationSpeedParam, value);
        }

        
        if (Mathf.Approximately(value, speedSlider.maxValue) && !isGameObjectActive)
        {
            StopAnimationAndActivateObject();
        }
    }

    void StopAnimationAndActivateObject()
    {
        
        if (targetAnimator != null)
        {
            targetAnimator.speed = 0;  
        }

        
        if (specialObject != null)
        {
            specialObject.SetActive(true);
            isGameObjectActive = true;
        }
    }
}
