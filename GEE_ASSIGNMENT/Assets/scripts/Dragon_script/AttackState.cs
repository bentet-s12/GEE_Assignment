using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent = animator.GetComponent<NavMeshAgent>();
        if (agent != null)
            agent.updateRotation = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 target = player.position;
        target.y = animator.transform.position.y;
        Vector3 dir = target - animator.transform.position;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            animator.transform.rotation = Quaternion.Slerp(
                animator.transform.rotation,
                rot,
                Time.deltaTime * 5f
            );
        }

        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance > 4.5f)
            animator.SetBool("isAttacking", false);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null)
            agent.updateRotation = true;
    }
}
