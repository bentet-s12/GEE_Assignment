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
    [HideInInspector] public JumpState jumpState;

    private bool isAiming = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Initialize states
        idleState = new IdleState(this);
        walkState = new WalkState(this);
        runState = new RunState(this);
        aimState = new AimState(this);
        jumpState = new JumpState(this);

        currentState = idleState;
        currentState.EnterState();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleAimToggle();
        HandleJumpInput();

        HandleMovement();

        currentState.UpdateState();
        ApplyGravity();
    }
    // ------------------ AIM TOGGLE ------------------
    private void HandleAimToggle()
    {
        // Press once to toggle aim
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = !isAiming; // flip the bool

            animator.SetBool("Aiming", isAiming);
            cameraScript.SetAiming(isAiming);

            // Maintain walking/running states
            animator.SetBool("Walking", animator.GetBool("Walking"));
            animator.SetBool("isJumping", animator.GetBool("isJumping"));
        }
    }

    // ------------------ MOVEMENT ------------------
    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // movement direction relative to camera
        Vector3 forward = cameraScript.GetCameraForwardFlat();
        Vector3 right = cameraScript.GetCameraRightFlat();
        Vector3 move = forward * v + right * h;

        // movement speed logic
        float speed = (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : walkSpeed;

        // slower movement while aiming
        if (isAiming)
            speed *= 0.7f;

        // movement apply
        if (move.magnitude > 0.1f)
        {
            controller.Move(move.normalized * speed * Time.deltaTime);

            // character faces camera direction while moving
            Vector3 lookDir = cameraScript.GetCameraForwardFlat();
            if (lookDir != Vector3.zero)
                transform.forward = lookDir;
        }
    }

    // ------------------ JUMP ------------------
    private void HandleJumpInput()
    {
        isGrounded = controller.isGrounded;

        // Reset when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        // Jump trigger
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = jumpForce;
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }

        // Falling transition
        if (velocity.y < 0 && !isGrounded)
        {
            animator.SetBool("isFalling", true);
        }
    }

    // ------------------ GRAVITY ------------------
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
