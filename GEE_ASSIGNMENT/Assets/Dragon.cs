using UnityEngine;

public class Dragon : MonoBehaviour
{
    public int HP = 100;
    public Animator animator;
    
    public void TakeDamage(int damageAmount)
    {
        HP = damageAmount;
        if(HP <= 0)
        {
            animator.SetTrigger("die");
        }
        else
        {
            animator.SetTrigger("getHit");
        }
    }
}
