using UnityEngine;

public class ForkliftController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 50f;

    public Transform forks; // Assign the forks object here
    public float forkSpeed = 2f;
    public float maxForkHeight = 5f; // Adjust this based on your model
    public float minForkHeight = 0.5f;

    void Update()
    {
        // Movement controls
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        transform.Translate(0, 0, move);
        transform.Rotate(0, turn, 0);

        // Fork controls
        if (Input.GetKey(KeyCode.UpArrow) && forks.localPosition.y < maxForkHeight)
        {
            forks.Translate(Vector3.up * forkSpeed * Time.deltaTime);
            Debug.Log("Forks moving up!");
        }
        else if (Input.GetKey(KeyCode.DownArrow) && forks.localPosition.y > minForkHeight)
        {
            forks.Translate(Vector3.down * forkSpeed * Time.deltaTime);
            Debug.Log("Forks moving down!");
        }
    }

}
