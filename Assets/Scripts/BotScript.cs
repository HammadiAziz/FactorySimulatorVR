using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField] private float waypointTolerance = 1.5f; 
    [SerializeField] private AnimationClip[] spectatingAnimations; 
    [SerializeField] private Transform[] dangerZones;

    private bool isSpectating = false; 
    private bool isInDangerZone = false; 

    void Start()
    {
        if (waypoints.Length > 0)
        {
            animator.SetBool("IsWalking", true);
            SetNextWaypoint();
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= waypointTolerance && !isSpectating && !isInDangerZone)
        {
            StartCoroutine(SpectateAtWaypoint());
        }
    }

    void SetNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        // Choose a random waypoint
        currentWaypointIndex = Random.Range(0, waypoints.Length);
        agent.SetDestination(waypoints[currentWaypointIndex].position);
        animator.SetBool("IsWalking", true); 
    }

    IEnumerator SpectateAtWaypoint()
    {
        isSpectating = true;
        agent.isStopped = true;
        animator.SetBool("IsWalking", false);

       
        int randomIndex = Random.Range(0, spectatingAnimations.Length);
        string animationName = spectatingAnimations[randomIndex].name;
        float animationDuration = spectatingAnimations[randomIndex].length;

        animator.SetTrigger(animationName);

        yield return new WaitForSeconds(animationDuration);

        animator.ResetTrigger(animationName);

        agent.isStopped = false;
        isSpectating = false;
        SetNextWaypoint();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DangerZone") && !isInDangerZone)  
        {
            PoliceEffects effects = other.GetComponent<PoliceEffects>();
            if (effects != null)
            {
                effects.StartEffects();
            }
            isInDangerZone = true; 
            StartCoroutine(DangerZoneSequence(other)); 
        }
    }

    IEnumerator DangerZoneSequence(Collider dangerZone)
    {
        agent.isStopped = true;
        animator.SetBool("IsWalking", false);

        NavMeshObstacle obstacle = dangerZone.GetComponent<NavMeshObstacle>();

        if (obstacle != null)
        {
            obstacle.enabled = true;
        }

        animator.SetTrigger("Terrified");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        animator.SetTrigger("TurnIntoRun");
        yield return new WaitForSeconds((animator.GetCurrentAnimatorClipInfo(0)[0].clip.length)/2);
        SetNextWaypoint();
        agent.isStopped = false;
        animator.SetBool("IsRunning", true);
        yield return new WaitForSeconds(3f);

        animator.SetBool("IsRunning", false);
        animator.SetBool("IsWalking", true);

        
        isInDangerZone = false;
    }
}
