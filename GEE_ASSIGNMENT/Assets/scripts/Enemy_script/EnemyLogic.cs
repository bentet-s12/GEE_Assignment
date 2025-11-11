using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    public Transform target;
    private EnemyRef enemyRef;

    private float swingingDistance;
    private float pathUpdateDeadline;

    private bool isSwinging = false;

    private float attackRangeBuffer = 0.5f;


    private void Awake()
    {
        enemyRef = GetComponent<EnemyRef>();
    }

    private void Start()
    {
        swingingDistance = enemyRef.agent.stoppingDistance;

        // Ensure hitbox is off
        if (enemyRef.hitbox != null)
            enemyRef.hitbox.SetActive(false);
    }

    private void Update()
    {
        if (target != null)
        {
            float dist = Vector3.Distance(transform.position, target.position);
            bool inRange = dist <= (swingingDistance + attackRangeBuffer);

            if (!isSwinging)
            {
                if (inRange)
                {
                    LookAtTarget();
                    StartCoroutine(SwingRoutine());
                }
                else
                {
                    UpdatePath();
                }
            }
            float speed = enemyRef.agent.velocity.magnitude;
            enemyRef.animator.SetFloat("speed", speed);
        }
    }

    private void LookAtTarget()
    {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    private void UpdatePath()
    {
        if (Time.time >= pathUpdateDeadline)
        {
            pathUpdateDeadline = Time.time + enemyRef.pathUpdateDelay;
            enemyRef.agent.SetDestination(target.position);
        }
    }

    private System.Collections.IEnumerator SwingRoutine()
    {
        isSwinging = true;

        // Stop moving
        enemyRef.agent.isStopped = true;
        enemyRef.animator.SetBool("swing", true);

        // 1. WINDUP
        yield return new WaitForSeconds(enemyRef.windupTime);

        // 2. SWING (damage active)
        if (enemyRef.hitbox != null)
            enemyRef.hitbox.SetActive(true);

        yield return new WaitForSeconds(enemyRef.swingTime);

        // Turn off hitbox after attack
        if (enemyRef.hitbox != null)
            enemyRef.hitbox.SetActive(false);

        // 3. COOLDOWN
        yield return new WaitForSeconds(enemyRef.cooldownTime);

        enemyRef.animator.SetBool("swing", false);

        // Resume movement
        enemyRef.agent.isStopped = false;

        isSwinging = false;
    }
}
