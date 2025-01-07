using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DefectMilkDetector : MonoBehaviour
{
    public AudioSource alarmSound;
    public float detectionRange = 1.2f;
    private string milkpackTag = "milkpack";
    private bool alarmActive = false;
    public TextMeshProUGUI alertText;
    public Canvas alertCanvas;
    public float alertDisplayTime = 4f;
    public float fadeDuration = 0.5f;
    public GameObject directionalArrowPrefab;

    private Dictionary<Transform, GameObject> spawnedArrows = new Dictionary<Transform, GameObject>();

    void Start()
    {
        if (alertCanvas != null)
        {
            alertCanvas.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (Collider detectedObject in detectedObjects)
        {
            if (detectedObject.CompareTag(milkpackTag))
            {
                AttachableObject attachableObject = detectedObject.GetComponent<AttachableObject>();
                if (attachableObject != null && attachableObject.HasDefect)
                {
                    HandleDefectiveMilkPack(detectedObject.transform);
                }
            }
        }
    }

    private void HandleDefectiveMilkPack(Transform milkPack)
    {
        if (!alarmActive)
        {
            StartCoroutine(TriggerAlarm());
        }
        ShowAlert("Defective Milk Detected!");
        HandleDirectionalArrow(milkPack);
    }

    private IEnumerator TriggerAlarm()
    {
        alarmActive = true;
        alarmSound.loop = true;
        alarmSound.Play();
        yield return new WaitForSeconds(4f);
        alarmSound.loop = false;
        alarmSound.Stop();
        alarmActive = false;
    }

    private void HandleDirectionalArrow(Transform milkPack)
    {
        if (directionalArrowPrefab != null)
        {
            if (!spawnedArrows.ContainsKey(milkPack))
            {
                GameObject arrow = Instantiate(directionalArrowPrefab, milkPack);
                arrow.transform.rotation = Quaternion.identity;
                StartCoroutine(FixArrowRotation(arrow));
                spawnedArrows[milkPack] = arrow;
                StartCoroutine(RemoveDirectionalArrow(milkPack, arrow));
            }
        }
    }

    private IEnumerator FixArrowRotation(GameObject arrow)
    {
        while (arrow != null)
        {
            arrow.transform.rotation = Quaternion.identity;
            yield return null;
        }
    }

    private IEnumerator RemoveDirectionalArrow(Transform milkPack, GameObject arrow)
    {
        yield return new WaitForSeconds(4f);

        if (arrow != null)
        {
            Destroy(arrow);
        }
        spawnedArrows.Remove(milkPack);
    }

    private void ShowAlert(string message)
    {
        if (alertText != null && alertCanvas != null)
        {
            alertCanvas.gameObject.SetActive(true);
            alertText.text = message;
            StartCoroutine(FadeInAlert());
            StartCoroutine(HideAlertAfterDelay(alertDisplayTime));
        }
    }

    private IEnumerator FadeInAlert()
    {
        float elapsedTime = 0f;
        Color startColor = alertText.color;
        startColor.a = 0;
        alertText.color = startColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            startColor.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            alertText.color = startColor;
            yield return null;
        }
        startColor.a = 1f;
        alertText.color = startColor;
    }

    private IEnumerator HideAlertAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(FadeOutAlert());
    }

    private IEnumerator FadeOutAlert()
    {
        float elapsedTime = 0f;
        Color startColor = alertText.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            startColor.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            alertText.color = startColor;
            yield return null;
        }

        startColor.a = 0f;
        alertText.color = startColor;
        alertCanvas.gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
