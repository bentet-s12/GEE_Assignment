public class JumpState : PlayerState
{
    private PlayerStateManager manager;

    public JumpState(PlayerStateManager manager)
    {
        this.manager = manager;
    }

    public void EnterState()
    {
        manager.animator.SetBool("isJumping", true);
        // optional: reduce horizontal movement control
    }

    public void UpdateState()
    {
        if (manager.controller.isGrounded && manager.velocity.y < 0)
        {
            manager.SwitchState(manager.idleState);
        }
    }

    public void ExitState()
    {
        manager.animator.SetBool("isJumping", false);
    }
}
