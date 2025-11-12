using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    public Transform target;
    private EnemyRef enemyRef;
    private CharacterController charController;

    private float verticalSpeed = 0f;
    private bool grounded = false;
    private bool isSwinging = false;

    public float gravity = -9.8f;
    public float dropGroundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    private float pathUpdateDeadline;

    private void Awake()
    {
        enemyRef = GetComponent<EnemyRef>();
        charController = GetComponent<CharacterController>();

        // disable navmesh agent initially
        enemyRef.agent.enabled = false;
    }

    private void Start()
    {
        if (enemyRef.hitbox != null)
            enemyRef.hitbox.SetActive(false);
    }

    private void Update()
    {
        if (!grounded)
        {
            ApplyGravityUntilGrounded();
            return;
        }
        if (grounded)
        {
            enemyRef.agent.enabled = true;
        }
        // normal combat/path logic once on ground
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);
        bool inRange = dist <= (enemyRef.agent.stoppingDistance + 0.5f);

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

    private void ApplyGravityUntilGrounded()
    {
        // Simple ground check
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, dropGroundCheckDistance, groundLayer))
        {
            grounded = true;
            verticalSpeed = 0f;

            // Align to ground
            Vector3 snapPos = hit.point;
            transform.position = snapPos;

            // enable navmesh agent now
            enemyRef.agent.enabled = true;
            enemyRef.agent.Warp(snapPos);
            return;
        }

        verticalSpeed += gravity * Time.deltaTime;
        charController.Move(Vector3.up * verticalSpeed * Time.deltaTime);
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

        enemyRef.agent.isStopped = true;
        enemyRef.animator.SetBool("swing", true);

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
}
