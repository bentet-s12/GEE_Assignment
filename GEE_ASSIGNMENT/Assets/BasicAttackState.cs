using UnityEngine;

public class BasicAttackState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Play attack animation hit event immediately
        //animator.GetComponent<Dragon>().DoBasicAttack();
    }
}
