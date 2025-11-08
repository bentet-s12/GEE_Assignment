using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerStateManager : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public FixedThirdPersonCamera cameraScript;
    [HideInInspector] public CharacterController controller;

    [Header("Speeds")]
    public float walkSpeed = 2.5f;
    public float runSpeed = 5f;

    [Header("Gravity")]
    public float gravity = -9.81f;
    private Vector3 velocity;
    private bool isGrounded;

    // States
    [HideInInspector] public PlayerState currentState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public WalkState walkState;
    [HideInInspector] public RunState runState;
    [HideInInspector] public AimState aimState;

    private bool isAiming = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        idleState = new IdleState(this);
        walkState = new WalkState(this);
        runState = new RunState(this);
        aimState = new AimState(this);

        currentState = idleState;
        currentState.EnterState();

        // ensure start not aiming
        animator.SetBool("Aiming", false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // toggle aim
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = !isAiming;
            animator.SetBool("Aiming", isAiming);
            cameraScript.SetAiming(isAiming);

            if (isAiming)
                SwitchState(aimState);
            else
                SwitchState(idleState);
        }

        // only move if not aiming
        if (!isAiming)
            HandleMovement();

        currentState.UpdateState();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Abs(h) < 0.1f && Mathf.Abs(v) < 0.1f)
            return;

        Vector3 forward = cameraScript.GetCameraForwardFlat();
        Vector3 right = cameraScript.GetCameraRightFlat();
        Vector3 move = forward * v + right * h;

        float speed = (currentState == runState) ? runSpeed : walkSpeed;
        controller.Move(move.normalized * speed * Time.deltaTime);

        Vector3 lookDir = cameraScript.GetCameraForwardFlat();
        if (lookDir != Vector3.zero)
            transform.forward = lookDir;
    }

    private void ApplyGravity()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        else
            velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    public void SwitchState(PlayerState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}
