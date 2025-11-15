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

    private void OnTriggerEnter (Collider other)
    {
        if (enemylogic.getSwinging() == true)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player hit for " + damage);
                playerdmg = other.GetComponent<stats>();
                if (playerdmg != null)
                {
                    playerdmg.takedmg(damage);
                }
                }
            }
        else
        {
            return;
        }
        if (hasHit) return;   
        
        //if (other.CompareTag("Player"))
        //{
          //  Debug.Log("Player hit for " + damage); 
        //}
    }
}
