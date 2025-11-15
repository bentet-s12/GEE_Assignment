using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : StateMachineBehaviour
{
    float timer;
    NavMeshAgent agent;
    Transform player;
    Dragon dragon;
    readonly List<Transform> Waypoints = new();

    const string PLAYER_TAG = "Player";

    void SetRandomDestination()
    {
        if (agent == null || !agent.isOnNavMesh || Waypoints.Count == 0)
            return;

        Transform target = Waypoints[Random.Range(0, Waypoints.Count)];
        agent.SetDestination(target.position);
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dragon = animator.GetComponentInParent<Dragon>();
        agent = animator.GetComponentInParent<NavMeshAgent>();

        if (dragon != null)
            dragon.SetDetectorActive(false);  // No attack colliders while patrolling

        GameObject p = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        if (p != null)
            player = p.transform;

        timer = 0f;

        GameObject go = GameObject.FindGameObjectWithTag("Waypoints");
        if (!go)
            return;

        Waypoints.Clear();
        foreach (Transform t in go.transform)
            Waypoints.Add(t);

        if (agent == null || dragon == null)
            return;

        if (!agent.isOnNavMesh)
            return;

        agent.speed = dragon.patrolSpeed;
        agent.isStopped = false;

        if (Waypoints.Count > 0)
            SetRandomDestination();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag(PLAYER_TAG);
            if (p != null) player = p.transform;
            else return;
        }

        if (agent == null || dragon == null || Waypoints.Count == 0)
            return;

        if (!agent.isOnNavMesh)
            return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            SetRandomDestination();
        }

        timer += Time.deltaTime;
        if (timer > 10f)
            animator.SetBool("isPatrolling", false);

        float distance = Vector3.Distance(player.position, dragon.transform.position);
        if (distance < dragon.chaseRange)
            animator.SetBool("isChasing", true);
    }
}
