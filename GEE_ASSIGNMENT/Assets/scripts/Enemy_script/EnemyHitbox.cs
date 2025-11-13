using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit for " + damage);
            // Hook up your damage system here

        }
    }
}
