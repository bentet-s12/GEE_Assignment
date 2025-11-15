using UnityEngine;

public class BasicAttackDetector : MonoBehaviour
{
    public Dragon dragon;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (dragon.CanBasicAttack)
            {
                dragon.DoBasicAttack();
            }
        }
    }

}
