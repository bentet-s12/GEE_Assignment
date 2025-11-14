using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int damage = 5;

    private bool hasHit = false;

    public void resetHit()
    {
        hasHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;   
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit for " + damage);
           

        }
    }
}
