using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    Dragon dragon;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = 5f;

        dragon = animator.GetComponent<Dragon>();
        agent.speed = dragon.chaseSpeed;

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float basicAttackRange = dragon.basicAttackRange;
        float flameAttackRange = dragon.flameAttackRange;


        // ====== CHASE THE PLAYER ======
        agent.SetDestination(player.position);

        float distance = Vector3.Distance(player.position, animator.transform.position);

        // ====== CHECK IF PLAYER IS IN FRONT ======
        Vector3 dirToPlayer = (player.position - animator.transform.position).normalized;
        float forwardDot = Vector3.Dot(animator.transform.forward, dirToPlayer);
        bool playerInFront = forwardDot > 0.3f;

        // ====== EXIT CHASE IF PLAYER TOO FAR ======
        if (distance > dragon.chaseRange)
        {
            animator.SetBool("isChasing", false);
            return;
        }

        // ====== FLAME ATTACK (LONG RANGE) ======
        if (distance <= flameAttackRange && distance > basicAttackRange && playerInFront)
        {
            animator.SetTrigger("FlameAttack");
            return;
        }

        // ====== BASIC ATTACK (CLOSE RANGE) ======
        if (distance <= basicAttackRange && playerInFront)
        {
            animator.SetTrigger("BasicAttack");
            return;
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }

    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
    }
}
