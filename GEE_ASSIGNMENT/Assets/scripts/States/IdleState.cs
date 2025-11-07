using UnityEngine;

public class IdleState : PlayerState
{
    private PlayerStateManager manager;
    public IdleState(PlayerStateManager manager) { this.manager = manager; }

    public void EnterState()
    {
        manager.animator.SetBool("Walking", false);
        manager.animator.SetBool("Running", false);
        manager.animator.SetFloat("hzinput", 0);
        manager.animator.SetFloat("vinput", 0);
    }

    public void UpdateState()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool run = Input.GetKey(KeyCode.LeftShift);

        if (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f)
        {
            if (run)
                manager.SwitchState(manager.runState);
            else
                manager.SwitchState(manager.walkState);
        }
    }

    public void ExitState() { }
}
