using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponentInParent<NavMeshAgent>();
        agent.isStopped = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.isStopped = false;
        animator.SetBool("isAttacking", false);
    }
}
