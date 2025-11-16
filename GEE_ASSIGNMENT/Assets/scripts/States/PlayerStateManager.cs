using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerStateManager : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public FixedThirdPersonCamera cameraScript;
    [HideInInspector] public CharacterController controller;

    [Header("Audio")]
    public AudioSource sfxSource;     // main audio source for character
    public AudioClip walkSFX;
    public AudioClip runSFX;
    public AudioClip jumpSFX;
    public AudioClip landSFX;

    [Header("Movement Speeds")]
    public float walkSpeed = 2.5f;
    public float runSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Gravity")]
    public float gravity = -9.81f;
    [HideInInspector] public Vector3 velocity;
    private bool isGrounded;
    [HideInInspector] public bool isFalling = false;


    // States
    [HideInInspector] public PlayerState currentState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public WalkState walkState;
    [HideInInspector] public RunState runState;
    [HideInInspector] public AimState aimState;
    [HideInInspector] public JumpState jumpState;

    [HideInInspector] public bool isAiming = false;

    public float getspeed()
    {
        return walkSpeed;
    }
    public void setspeed(float addspeed)
    {
        walkSpeed += addspeed;
        runSpeed += addspeed;

    }
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

            // Maintain walking, jumping 
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

        float speed = (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : walkSpeed;

        if (isAiming)
            speed *= 0.7f;

        // MOVE
        if (move.magnitude > 0.1f)
        {
            controller.Move(move.normalized * speed * Time.deltaTime);

            // movement rotation
            Vector3 lookDir = cameraScript.GetCameraForwardFlat();
            if (lookDir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
            }
        }

        // ⭐ Rotate player with camera even when standing still and aiming
        if (isAiming && move.magnitude < 0.1f)
        {
            Vector3 aimDir = cameraScript.GetCameraForwardFlat();
            if (aimDir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(aimDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
            }
        }
    }


    // ------------------ JUMP ------------------
    private void HandleJumpInput()
    {
        isGrounded = controller.isGrounded;

        // ---------------- JUMP ----------------
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (jumpSFX != null)
                sfxSource.PlayOneShot(jumpSFX);
            velocity.y = jumpForce;

            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
            isFalling = false;
            return;
        }

        // ---------------- FALLING ----------------
        if (!isGrounded && velocity.y < 0)
        {
            isFalling = true;
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
        }

        // ---------------- LANDING ----------------
        if (isGrounded && velocity.y < 0)
        {
            if (landSFX != null && animator.GetBool("isFalling"))
                sfxSource.PlayOneShot(landSFX);

            velocity.y = -2f;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            isFalling = false;
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
