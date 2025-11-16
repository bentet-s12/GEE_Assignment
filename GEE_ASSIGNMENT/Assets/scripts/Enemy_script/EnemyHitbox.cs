using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] private EnemyLogic enemylogic;
    [SerializeField] private stats playerdmg;

    public int damage = 5;

    private bool hasHit = false;

    public void resetHit()
    {
        hasHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Don't hit twice in one swing
        if (hasHit) return;

        // Enemy must be swinging
        if (!enemylogic.getSwinging()) return;

        // Check if player entered
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit for " + damage);

            playerdmg = other.GetComponent<stats>();
            if (playerdmg != null)
            {
                playerdmg.takedmg(damage);
                hasHit = true; // Mark that this swing already hit
            }
        }
    }
}
