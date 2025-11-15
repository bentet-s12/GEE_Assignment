using UnityEngine;
using UnityEngine.AI;

public class ChaseState2 : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    Dragon dragon;

    const string PLAYER_TAG = "Player";

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dragon = animator.GetComponentInParent<Dragon>();
        agent = animator.GetComponentInParent<NavMeshAgent>();

        GameObject p = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        if (p != null)
            player = p.transform;

        if (dragon != null)
            dragon.SetDetectorActive(true);

        if (agent != null && dragon != null && agent.isOnNavMesh)
        {
            agent.speed = dragon.chaseSpeed;
            agent.isStopped = false;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag(PLAYER_TAG);
            if (p != null) player = p.transform;
            else return;
        }

        if (agent == null || dragon == null)
            return;

        if (!agent.isOnNavMesh)
            return;

        agent.SetDestination(player.position);

        float distance = Vector3.Distance(player.position, dragon.transform.position);

        if (distance > dragon.chaseRange)
            animator.SetBool("isChasing", false);

        if (distance < dragon.flameAttackRange)
            animator.SetBool("isAttacking", true);
        else
            animator.SetBool("isAttacking", false);
    }
}
