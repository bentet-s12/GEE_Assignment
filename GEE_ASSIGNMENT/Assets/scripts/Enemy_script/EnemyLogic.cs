using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    private EnemyRef enemyRef;

    [Header("Attack Settings")]
    private float swingingDistance;
    private bool isSwinging = false;
    private float pathUpdateDeadline;
    private float attackRangeBuffer = 0.2f;

    [Header("Detection Settings")]
    public float detectionRange = 12f;

    [Header("Wander Settings")]
    public float wanderRadius = 8f;
    public float wanderDelay = 3f;
    public float obstacleRange = 3f;
    public float rotSpeed = 8f;

    [Header("Stuck Detection")]
    public float stuckCheckInterval = 2f;
    public float minMoveDistance = 0.5f;

    private float wanderTimer;
    private Quaternion wanderDirection;
    private Vector3 lastPosition;
    private float stuckTimer;

    private void Awake()
    {
        enemyRef = GetComponent<EnemyRef>();

        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                target = p.transform;
        }
    }

    private void Start()
    {
        swingingDistance = enemyRef.agent.stoppingDistance;
        if (enemyRef.hitbox != null)
            enemyRef.hitbox.SetActive(false);

        wanderTimer = wanderDelay;
        newWander();

        lastPosition = transform.position;
        stuckTimer = 0f;
    }

    private void Update()
    {
        if (target != null)
        {
            float dist = Vector3.Distance(transform.position, target.position);

            // If target is within detection range, chase/attack
            if (dist <= detectionRange)
            {
                ChaseAndAttack(dist);
            }
            else
            {
                Wander();
            }
        }
        else
        {
            Wander();
        }

        // update animation speed
        float speed = enemyRef.agent.velocity.magnitude;
        enemyRef.animator.SetFloat("speed", speed); //this is the normal walking animation
    }


    private void ChaseAndAttack(float dist)
    {
        bool inRange = dist <= (swingingDistance + attackRangeBuffer);

        if (!isSwinging && !inRange)
        {
            enemyRef.agent.isStopped = false;
            UpdatePathToTarget();
        }

        if (!isSwinging && inRange)
        {
            LookAtTarget();
            StartCoroutine(SwingRoutine());
        }
    }

    private void LookAtTarget()
    {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        if (lookPos.sqrMagnitude > 0.001f)
        {
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
        }
    }

    private void UpdatePathToTarget()
    {
        if (Time.time >= pathUpdateDeadline)
        {
            pathUpdateDeadline = Time.time + enemyRef.pathUpdateDelay;
            if (enemyRef.agent.enabled)
                enemyRef.agent.SetDestination(target.position);
        }
    }

    private IEnumerator SwingRoutine()
    {
        isSwinging = true;

        enemyRef.agent.isStopped = true;
        enemyRef.animator.SetBool("swing", true); //this is the animation of "swinging"

        if (enemyRef.hitbox != null)
            enemyRef.hitbox.GetComponent<EnemyHitbox>().resetHit();

        yield return new WaitForSeconds(enemyRef.windupTime);

        if (enemyRef.hitbox != null)
            enemyRef.hitbox.SetActive(true);

        yield return new WaitForSeconds(enemyRef.swingTime);

        if (enemyRef.hitbox != null)
            enemyRef.hitbox.SetActive(false);

        yield return new WaitForSeconds(enemyRef.cooldownTime);

        enemyRef.animator.SetBool("swing", false);
        enemyRef.agent.isStopped = false;

        isSwinging = false;
    }


    private void Wander()
    {
        if (isSwinging) return;

        wanderTimer += Time.deltaTime;
        stuckTimer += Time.deltaTime;

        // Avoid obstacles
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, transform.forward);
        if (Physics.SphereCast(ray, 0.5f, out RaycastHit hit, obstacleRange))
        {
            float angle = Random.Range(-110f, 110f);
            wanderDirection = Quaternion.LookRotation(Quaternion.Euler(0, angle, 0) * transform.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, wanderDirection, rotSpeed * Time.deltaTime);
            wanderTimer = 0;
        }

        // Pick new wander point periodically
        if (wanderTimer >= wanderDelay)
        {
            newWander();
            wanderTimer = 0;
        }

        //if the enemy is not moving much after a few seconds
        if (stuckTimer >= stuckCheckInterval)
        {
            float movedDistance = Vector3.Distance(transform.position, lastPosition);
            if (movedDistance < minMoveDistance)
            {
                // Consider stuck → pick a new direction
                float randomAngle = Random.Range(-180f, 180f);
                wanderDirection = Quaternion.Euler(0, randomAngle, 0) * transform.rotation;
                transform.rotation = wanderDirection;

                newWander();
            }
            lastPosition = transform.position;
            stuckTimer = 0f;
        }
    }

    private void newWander()
    {
        Vector3 forwardPoint = Random.insideUnitSphere * wanderRadius + transform.position;
        if (NavMesh.SamplePosition(forwardPoint, out NavMeshHit navHit, wanderRadius, NavMesh.AllAreas))
        {
            enemyRef.agent.isStopped = false;
            enemyRef.agent.SetDestination(navHit.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
