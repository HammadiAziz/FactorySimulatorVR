using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TutorialManager : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject milkPack;
    public Transform navMeshLocation;
    private GameObject currentArrow;

    private enum TutorialState { Start, GrabMilk, PlaceMilk, Complete }
    private TutorialState currentState = TutorialState.Start;

    void Start()
    {
        StartCoroutine(ShowNextTutorialStep());
    }

    private IEnumerator ShowNextTutorialStep()
    {
        while (currentState != TutorialState.Complete)
        {
            switch (currentState)
            {
                case TutorialState.Start:
                    Debug.Log("Tutorial Start");
                    currentState = TutorialState.GrabMilk;
                    break;

                case TutorialState.GrabMilk:
                    Debug.Log("Point to milk pack to grab it.");
                    SpawnArrow(milkPack.transform);

                    yield return new WaitUntil(() => IsGrabbed());
                    Destroy(currentArrow);

                    currentState = TutorialState.PlaceMilk;
                    break;

                case TutorialState.PlaceMilk:
                    Debug.Log("Point to NavMesh to place milk pack.");
                    SpawnArrow(navMeshLocation);
                    yield return new WaitUntil(() => IsPlaced());
                    Destroy(currentArrow);

                    
                    currentState = TutorialState.Complete;
                    break;

                default:
                    yield break;
            }
        }

        Debug.Log("Tutorial Complete!");
    }

    private void SpawnArrow(Transform target)
    {
        if (arrowPrefab != null)
        {
            currentArrow = Instantiate(arrowPrefab, target.position, Quaternion.identity);
            currentArrow.transform.SetParent(target);
        }
    }

    private bool IsGrabbed()
    {
        if (milkPack.TryGetComponent(out AttachableObject attachable))
        {
            return attachable.GetComponent<XRGrabInteractable>().isSelected;
        }
        return false;
    }

    private bool IsPlaced()
    {
        float distance = Vector3.Distance(milkPack.transform.position, navMeshLocation.position);
        return distance < 0.5f;
    }
}
