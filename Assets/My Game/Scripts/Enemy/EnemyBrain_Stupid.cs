using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("TrannChanhh/EnemyBrain_Stupid")]
public class EnemyBrain_Stupid : MonoBehaviour
{
    // Static counter for the number of enemies targeting the player
    private static int enemiesTargetingPlayer = 0;

    [Header("Patrol")]
    public Transform[] wayPoint;
    public Transform target;
    private NavMeshAgent navMeshAgent;
    private PlayerHealth health;

    private EnemyReferences enemyReferences;
    private float attackingDistance;
    private float pathUpdateTimer = 0.5f;
    private int currentWaypointIndex;
    private Vector3 originalPosition;

    [Header("Behavior")]
    public float chaseRange = 20f;  // Distance to start chasing
    public float returnRange = 25f; // Distance to start returning
    public float runTriggerDistance = 20f;

    private enum State { Patrolling, Chasing, Returning }
    private State currentState = State.Patrolling;

    private void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (enemyReferences != null && enemyReferences.animator == null)
        {
            enemyReferences.animator = GetComponent<Animator>();
        }
    }

    private void Start()
    {
        health = FindAnyObjectByType<PlayerHealth>();
        attackingDistance = navMeshAgent.stoppingDistance;

        if (originalPosition == Vector3.zero)
        {
            originalPosition = transform.position;
        }
    }

    private void Update()
    {
        if (health.isDeadTriggered == true)
        {
            navMeshAgent.ResetPath();
            currentState = State.Patrolling;
            enemyReferences.animator.SetBool("IsRun", false);
            enemyReferences.animator.SetBool("IsWalk", false);
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                if (distanceToTarget <= chaseRange)
                {
                    currentState = State.Chasing;
                    navMeshAgent.ResetPath();
                    enemyReferences.animator.SetBool("IsWalk", false);
                    AudioManager.Instance.enemySource.PlayOneShot(AudioManager.Instance.enemyFootsteps);
                    
                }
                break;

            case State.Chasing:
                ChasePlayer(distanceToTarget);
                if (distanceToTarget > returnRange)
                {
                    currentState = State.Returning;
                    navMeshAgent.ResetPath();
                    AudioManager.Instance.enemySource.PlayOneShot(AudioManager.Instance.enemyNoise);
                    enemyReferences.animator.SetBool("IsRun", true);
                    enemyReferences.animator.SetBool("IsWalk", false);
                    
                }
                break;

            case State.Returning:
                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.3f)
                {
                    navMeshAgent.destination = originalPosition;
                    currentState = State.Patrolling;
                    enemyReferences.animator.SetBool("IsRun", false);
                    enemyReferences.animator.SetBool("IsWalk", true);
                    
                }
                break;
        }
    }

    private void OnPlayerDetected()
    {
        // Increase the count and start detection music if this is the first enemy to detect the player
        if (enemiesTargetingPlayer == 0)
        {
            AudioManager.Instance.StartDetectionMusic();
            AudioManager.Instance.enemySource.PlayOneShot(AudioManager.Instance.enemyNoise);
        }
        enemiesTargetingPlayer++;
    }

    private void OnPlayerLost()
    {
        // Decrease the count and stop detection music if no enemies are targeting the player
        enemiesTargetingPlayer = Mathf.Max(0, enemiesTargetingPlayer - 1);
        if (enemiesTargetingPlayer == 0)
        {
            AudioManager.Instance.StopDetectionMusic();
        }
    }

    private void Patrol()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.3)
        {
            GotoNext();
        }
    }

    private void GotoNext()
    {
        if (wayPoint.Length == 0) return;
        navMeshAgent.destination = wayPoint[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % wayPoint.Length;
        enemyReferences.animator.SetBool("IsWalk", true);

        if (!AudioManager.Instance.enemySource.isPlaying)
        {
            AudioManager.Instance.enemySource.PlayOneShot(AudioManager.Instance.enemyFootsteps);
        }
    }

    private void ChasePlayer(float distanceToTarget)
    {
        if (pathUpdateTimer <= 0)
        {
            navMeshAgent.destination = target.position;
            pathUpdateTimer = 0.5f;
        }
        else
        {
            pathUpdateTimer -= Time.deltaTime;
        }

        if (distanceToTarget < runTriggerDistance)
        {
            enemyReferences.animator.SetBool("IsRun", true);
            if (!AudioManager.Instance.enemySource.isPlaying)
            {
                AudioManager.Instance.enemySource.PlayOneShot(AudioManager.Instance.enemyNoise);
            }
        }
        else
        {
            enemyReferences.animator.SetBool("IsRun", false);
        }

        LookAtTarget();
    }

    private void LookAtTarget()
    {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }
}
