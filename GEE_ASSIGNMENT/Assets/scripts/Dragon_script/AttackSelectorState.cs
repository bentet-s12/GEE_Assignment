using UnityEngine;
using UnityEngine.AI;

public class AttackSelectorState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;
    DragonPoints points;

    // Ranges
    public float basicRange = 3.5f;     // Claw/Bite range
    public float flameRange = 7f;       // Flame attack distance

    // Cooldowns
    float basicCooldown = 1.0f;
    float flameCooldown = 2.0f;

    float lastBasic = -999f;
    float lastFlame = -999f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        points = animator.GetComponent<DragonPoints>();

        agent.isStopped = true;
        agent.updateRotation = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Rotate toward player (only Y axis)
        Vector3 dir = player.position - animator.transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, rot, Time.deltaTime * 7f);
        }

        // Distance from dragon mouth (AttackPoint)
        float distance = Vector3.Distance(player.position, points.attackPoint.position);

        // Too far? leave attack mode
        if (distance > flameRange + 1f)
        {
            animator.SetBool("isAttacking", false);
            return;
        }

        bool canBasic = distance <= basicRange && Time.time > lastBasic + basicCooldown;
        bool canFlame = distance <= flameRange && Time.time > lastFlame + flameCooldown;

        // PRIORITY: Flame > Claw
        if (canFlame)
        {
            animator.SetTrigger("FlameAttack");
            lastFlame = Time.time;
            return;
        }

        if (canBasic)
        {
            animator.SetTrigger("ClawAttack"); // using your animation’s name
            lastBasic = Time.time;
            return;
        }

        // If both are on cooldown, stay in selector but do nothing
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.isStopped = false;
        agent.updateRotation = true;
    }
}
