using UnityEngine;

public class WalkState : PlayerState
{
    private PlayerStateManager manager;
    public WalkState(PlayerStateManager manager) { this.manager = manager; }

    public void EnterState()
    {
        manager.animator.SetBool("Walking", true);
        manager.animator.SetBool("Running", false);
    }

    public void UpdateState()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool run = Input.GetKey(KeyCode.LeftShift);

        manager.animator.SetFloat("hzinput", h);
        manager.animator.SetFloat("vinput", v);

        if (Mathf.Abs(h) < 0.1f && Mathf.Abs(v) < 0.1f)
        {
            manager.SwitchState(manager.idleState);
            return;
        }

        if (run)
        {
            manager.SwitchState(manager.runState);
            return;
        }
    }

    public void ExitState()
    {
        manager.animator.SetBool("Walking", false);
    }
}
