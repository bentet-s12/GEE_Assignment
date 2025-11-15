using UnityEngine;

public class FlameAttackDetector : MonoBehaviour
{
    public Dragon dragon;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (dragon.CanFlameAttack)
            {
                dragon.DoFlameAttack();
            }
        }
    }

}
