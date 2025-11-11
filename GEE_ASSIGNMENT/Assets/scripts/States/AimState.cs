using UnityEngine;

public class AimState : PlayerState
{
    private PlayerStateManager manager;

    public AimState(PlayerStateManager manager)
    {
        this.manager = manager;
    }

    public void EnterState()
    {
        manager.animator.SetBool("Aiming", true);
        
    }

    public void UpdateState()
    {
        // Player just faces camera while aiming
        Vector3 lookDir = manager.cameraScript.GetCameraForwardFlat();
        if (lookDir != Vector3.zero)
            manager.transform.forward = lookDir;
    }

    public void ExitState()
    {
        manager.animator.SetBool("Aiming", false);
    }
}
