using UnityEngine;

public class Dragon2 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float patrolSpeed = 2.5f;
    public float chaseSpeed = 5f;

    [Header("Detection Ranges")]
    public float chaseRange = 8f;
    public float basicAttackRange = 4f;
    public float flameAttackRange = 10f;

    [Header("Damage Settings")]
    public int basicAttackDamage = 20;
    public int flameDamage = 10;

    [Header("Dragon Health")]
    public int dragonHealth = 200;

    [Header("Attack Cooldowns")]
    public float basicAttackCooldown = 1.5f;
    public float flameAttackCooldown = 3f;

    private bool canBasicAttack = true;
    private bool canFlameAttack = true;


    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // ======== CLOSE RANGE ATTACK ========
    public void DoBasicAttack()
    {
        if (!canBasicAttack)
            return;

        FacePlayer();   // ← add this

        canBasicAttack = false;

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

        FacePlayer();   // ← add this

        canFlameAttack = false;

        stats playerStats = player.GetComponent<stats>();
        if (playerStats != null)
            playerStats.takedmg(flameDamage);

        Debug.Log("Dragon used FLAME ATTACK! Damage: " + flameDamage);

        Invoke(nameof(ResetFlameAttack), flameAttackCooldown);
    }


    public void FacePlayer()
    {
        if (player == null) return;

        // Direction to player
        Vector3 direction = (player.position - transform.position);
        direction.y = 0; // Keep dragon upright

        if (direction.magnitude < 0.1f)
            return;

        // Smooth rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
    }


    private void ResetFlameAttack()
    {
        canFlameAttack = true;
    }


    public void TakeDamage(int amount)
    {
        dragonHealth -= amount;
        Debug.Log("Dragon took " + amount + " damage!");

        if (dragonHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Dragon DIED!");
        Destroy(gameObject);
    }
}
