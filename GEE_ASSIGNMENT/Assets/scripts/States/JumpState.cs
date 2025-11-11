using UnityEngine;

public class JumpState : PlayerState
{
    private PlayerStateManager manager;
    private bool hasJumped = false;

    public JumpState(PlayerStateManager manager)
    {
        this.manager = manager;
    }

    public void EnterState()
    {
        // Add upward force
        if (!hasJumped)
        {
            manager.velocity.y = manager.jumpForce;
            hasJumped = true;
        }

        manager.animator.SetBool("isJumping", true);
        manager.animator.SetBool("isFalling", false);
    }

    public void UpdateState()
    {
        // Apply gravity
        manager.velocity.y += manager.gravity * Time.deltaTime;
        manager.controller.Move(manager.velocity * Time.deltaTime);

        // Switch to falling when velocity starts going down
        if (manager.velocity.y < 0)
        {
            manager.animator.SetBool("isJumping", false);
            manager.animator.SetBool("isFalling", true);
        }

        // When grounded again → return to correct state
        if (manager.controller.isGrounded && manager.velocity.y < 0)
        {
            hasJumped = false;
            manager.animator.SetBool("isFalling", false);

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    manager.SwitchState(manager.runState);
                else
                    manager.SwitchState(manager.walkState);
            }
            else
            {
                manager.SwitchState(manager.idleState);
            }
        }
    }

    public void ExitState()
    {
        manager.animator.SetBool("isJumping", false);
        manager.animator.SetBool("isFalling", false);
    }
}
