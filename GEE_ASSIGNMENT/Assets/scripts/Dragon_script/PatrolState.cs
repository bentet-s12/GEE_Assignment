using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : StateMachineBehaviour
{
    float timer;
    List<Transform> Waypoints = new List<Transform>();
    NavMeshAgent agent;
    Transform player;
    float chaseRange = 8;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 2.5f;
        timer = 0;

        GameObject go = GameObject.FindGameObjectWithTag("Waypoints");
        if (go == null)
        {
            Debug.LogError("No GameObject with tag 'Waypoints' found in the scene!");
            return;
        }

        Waypoints.Clear();
        foreach (Transform t in go.transform)
            Waypoints.Add(t);

        if (Waypoints.Count == 0)
        {
            Debug.LogError("No child waypoints found under the 'Waypoints' object!");
            return;
        }

        agent.SetDestination(Waypoints[Random.Range(0, Waypoints.Count)].position);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Waypoints.Count == 0) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
            agent.SetDestination(Waypoints[Random.Range(0, Waypoints.Count)].position);

        timer += Time.deltaTime;
        if (timer > 10)
            animator.SetBool("isPatrolling", false);

        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < chaseRange)
            animator.SetBool("isChasing", true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }
}
