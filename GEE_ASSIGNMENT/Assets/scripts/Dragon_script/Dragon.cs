using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Dragon : MonoBehaviour
{
    public ParticleSystem FireBreathVFX;

    private Animator anim;

    [Header("Movement Settings")]   
    public float patrolSpeed = 2.5f;
    public float chaseSpeed = 3f;

    [Header("Detection Ranges")]
    public float chaseRange = 2f;
    public float basicAttackRange = 4f;
    public float flameAttackRange = 10f;

    [Header("Damage Settings")]
    public int basicAttackDamage = 20;
    public int flameDamage = 10;

    [Header("Dragon Health")]
    public int maxHealth = 200;
    private int dragonHealth;
    private int nextFlinchThreshold;


    [Header("Attack Cooldowns")]
    public float basicAttackCooldown = 1.5f;
    public float flameAttackCooldown = 3f;

    private bool canBasicAttack = true;
    private bool canFlameAttack = true;

    [SerializeField]private int currentRoom;
    [SerializeField] private gamelogic roomscaler;
    public bool CanBasicAttack => canBasicAttack;
    public bool CanFlameAttack => canFlameAttack;

    [Header("Attack Detectors")]
    public GameObject basicDetector;
    public GameObject flameDetector;

    Transform player;

    const string PLAYER_TAG = "Player";

    [SerializeField] private levelling_logic exp;

    void Start()
    {
        exp = GameObject.FindGameObjectWithTag("gameManager").GetComponent<levelling_logic>();
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
        anim = GetComponent<Animator>();

        // Initialize health
        if (roomscaler != null)
        {
            currentRoom = roomscaler.getCurrentRoom();
            dragonHealth = maxHealth * (currentRoom / 10);
            basicAttackDamage = basicAttackDamage * (currentRoom / 10);
            flameDamage = flameDamage * (currentRoom / 10);
        }
        else
        {
            return;
        }

        // First reaction at 75% HP
        nextFlinchThreshold = maxHealth - (maxHealth / 4);
    }

    // ======== CLOSE RANGE ATTACK ========
    public void DoBasicAttack()
    {
        if (!canBasicAttack)
            return;

        FacePlayer();

        canBasicAttack = false;

        anim.SetTrigger("BasicAttack");

        stats playerStats = player.GetComponent<stats>();
        if (playerStats != null)
            playerStats.takedmg(basicAttackDamage);

        Debug.Log("Dragon used BASIC ATTACK! Damage: " + basicAttackDamage);

        Invoke(nameof(ResetBasicAttack), basicAttackCooldown);
    }


    private void ResetBasicAttack()
    {
        canBasicAttack = true;
    }

    // ======== FLAME ATTACK (RANGE ATTACK) ========
    public void DoFlameAttack()
    {
        if (!canFlameAttack)
            return;

        FacePlayer();
        canFlameAttack = false;

        anim.SetTrigger("FlameAttack");

        stats playerStats = player.GetComponent<stats>();
        if (playerStats != null)
            playerStats.takedmg(flameDamage);

        Debug.Log("Dragon used Flame Attack! Damage: " + flameDamage);

        Invoke(nameof(ResetFlameAttack), flameAttackCooldown);
    }

    public void StartFlameVFX()
    {
        FireBreathVFX.Play();
    }

    public void StopFlameVFX()
    {
        FireBreathVFX.Stop();
        FireBreathVFX.Clear();
    }

    private void ResetFlameAttack()
    {
        canFlameAttack = true;
    }


    public void FacePlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position);
        direction.y = 0;

        if (direction.magnitude < 0.1f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
    }

    public void SetDetectorActive(bool active)
    {
        if (basicDetector != null)
            basicDetector.SetActive(active);

        if (flameDetector != null)
            flameDetector.SetActive(active);
    }
    
    public void TakeDamage(int amount)
    {
        if (dragonHealth <= 0)
            return;

        dragonHealth -= amount;
        Debug.Log("Dragon took " + amount + " damage!");

        // HP-based hit reaction (25% intervals)
        if (dragonHealth <= nextFlinchThreshold && dragonHealth > 0)
        {
            TriggerHitReaction();

            // Move threshold down by another 25%
            nextFlinchThreshold -= (maxHealth / 4);
        }

        if (dragonHealth <= 0)
        {
            Die();
        }
    }

    private void TriggerHitReaction()
    {
        anim.SetTrigger("getHit");
    }


    void Die()
    {
        Debug.Log("Dragon DIED!");

        anim.SetTrigger("die");

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
            
        this.enabled = false;

        if (basicDetector != null) basicDetector.SetActive(false);
        if (flameDetector != null) flameDetector.SetActive(false);
        if(exp != null)
        {
            exp.bosslvlup();

        }
        Destroy(gameObject, 6f);
    }

}
