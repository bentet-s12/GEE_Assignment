using UnityEngine;
using UnityEngine.AI;

public class EnemyRef : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Stat")]

    [Header("Attack Settings")]
    public float windupTime = 0.4f;
    public float swingTime = 0.3f;
    public float cooldownTime = 1.0f;

    public GameObject hitbox;


    public float pathUpdateDelay = 0.2f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
}
