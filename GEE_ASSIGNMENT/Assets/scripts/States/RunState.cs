using UnityEngine;

public class RunState : PlayerState
{
    private PlayerStateManager manager;
    public RunState(PlayerStateManager manager) { this.manager = manager; }

    public void EnterState()
    {
        manager.animator.SetBool("Running", true);
        manager.animator.SetBool("Walking", false);
    }

    public void UpdateState()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool run = Input.GetKey(KeyCode.LeftShift);

        manager.animator.SetFloat("hzinput", h);
        manager.animator.SetFloat("vinput", v);

        if (!run && (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f))
        {
            manager.SwitchState(manager.walkState);
            return;
        }

        if (Mathf.Abs(h) < 0.1f && Mathf.Abs(v) < 0.1f)
        {
            manager.SwitchState(manager.idleState);
            return;
        }
    }

    public void ExitState()
    {
        manager.animator.SetBool("Running", false);
    }
}
