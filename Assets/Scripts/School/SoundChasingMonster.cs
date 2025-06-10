using UnityEngine;
using UnityEngine.AI;

public class SoundChasingMonster : MonoBehaviour
{
    public NavMeshAgent agent;
    public float patrolRadius = 10f;
    public float waitTimeAtPoint = 2f;
    public float hearingRange = 20f;

    private bool chasingSound = false;
    private Vector3 soundTarget;
    private float waitTimer = 0f;

    private void OnEnable()
    {
        SoundManager.OnSoundMade += OnSoundHeard;
    }

    private void OnDisable()
    {
        SoundManager.OnSoundMade -= OnSoundHeard;
    }

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        SetNewPatrolDestination();
    }

    void Update()
    {
        if (chasingSound)
        {
            float dist = Vector3.Distance(transform.position, soundTarget);
            if (dist < 1.5f)
            {
                chasingSound = false;
                SetNewPatrolDestination();
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtPoint)
            {
                SetNewPatrolDestination();
                waitTimer = 0f;
            }
        }
    }

    void SetNewPatrolDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void OnSoundHeard(Vector3 soundPos)
    {
        float distance = Vector3.Distance(transform.position, soundPos);
        if (distance <= hearingRange)
        {
            chasingSound = true;
            soundTarget = soundPos;
            agent.SetDestination(soundTarget);
            Debug.Log("Monster heard sound at " + soundPos);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Hearing range (Red)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hearingRange);

        // Patrol radius (Green)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
