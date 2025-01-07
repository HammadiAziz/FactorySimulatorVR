using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class AttachableObject : MonoBehaviour
{
    public string attachPointTag = "AttachPoint";
    public string detachPointTag = "DetachPoint";
    public string detachDefectTag = "DetachDefect";
    private string defectTag = "defect";
    public bool HasDefect = false;
    private bool isAttached = false;
    private Transform attachPoint;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private bool canAttachAgain = true;
    public float attachDelay = 20f;
    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnGrabReleased);
        }

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = false;
        rb.useGravity = true;

        CheckChildObjects(transform);
    }

    void Update()
    {
        if (isAttached && attachPoint != null)
        {
            transform.position = attachPoint.position;
            transform.rotation = attachPoint.rotation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(attachPointTag) && canAttachAgain)
        {
            AttachToPoint(other.transform);
        }

        if (other.CompareTag(detachPointTag) && !HasDefect)
        {
            Detach(detachPointTag);
        }
        else if (other.CompareTag(detachDefectTag) && HasDefect)
        {
            Detach(detachDefectTag);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isAttached)
        {
            if (other.CompareTag(detachPointTag) && !HasDefect)
            {
                Detach(detachPointTag);
            }
            else if (other.CompareTag(detachDefectTag) && HasDefect)
            {
                Detach(detachDefectTag);
            }
        }
    }

    private void AttachToPoint(Transform point)
    {
        if (canAttachAgain)
        {
            attachPoint = point;
            isAttached = true;

            rb.isKinematic = true;
            rb.useGravity = false;

            StartCoroutine(AttachDelay());
        }
    }

    private void Detach(string detachTag)
    {
        Debug.Log($"Detaching to: {detachTag}");

        isAttached = false;
        attachPoint = null;
        rb.isKinematic = false;
        rb.useGravity = true;
        if (detachTag == detachDefectTag)
        {
            transform.position += new Vector3(-1f, 0, 0);
        }
        StartCoroutine(AttachDelay());
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("OnGrab called");

        if (isAttached)
        {
            Detach(HasDefect ? detachDefectTag : detachPointTag);
        }
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private void OnGrabReleased(SelectExitEventArgs args)
    {
        Debug.Log("OnGrabReleased called");
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private IEnumerator AttachDelay()
    {
        canAttachAgain = false;
        yield return new WaitForSeconds(attachDelay);
        canAttachAgain = true;
    }

    private void CheckChildObjects(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(defectTag))
            {
                HasDefect = true;
                return;
            }
        }
        HasDefect = false;
    }
}
