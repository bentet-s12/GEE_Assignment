using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerStateManager : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public FixedThirdPersonCamera cameraScript;
    [HideInInspector] public CharacterController controller;

    [Header("Movement Speeds")]
    public float walkSpeed = 2.5f;
    public float runSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Gravity")]
    public float gravity = -9.81f;
    [HideInInspector] public Vector3 velocity;
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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleAimToggle();
        HandleJumpInput();

        currentState.UpdateState();
        ApplyGravity();

        if (!isAiming)
            HandleMovement();
    }

    // ------------------ AIM TOGGLE ------------------
    private void HandleAimToggle()
    {
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
    }

    // ------------------ MOVEMENT ------------------
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

    // ------------------ JUMP ------------------
    private void HandleJumpInput()
    {
        isGrounded = controller.isGrounded;

        // Reset on ground
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isAiming)
        {
            velocity.y = jumpForce;
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }

        // Falling
        if (velocity.y < 0 && !isGrounded)
        {
            animator.SetBool("isFalling", true);
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // ------------------ STATE SWITCH ------------------
    public void SwitchState(PlayerState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}
