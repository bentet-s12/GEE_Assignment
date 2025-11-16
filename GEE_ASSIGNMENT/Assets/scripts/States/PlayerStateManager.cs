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

    [Header("Double Jump Ability")]
    public bool canDoubleJump = false;
    private bool hasDoubleJumped = false;

    // ================= TELEPORT ABILITY =================
    [Header("Teleport Ability")]
    public bool canTeleport = false;        // unlocked ability
    public float teleportCooldown = 6f;     // starting cooldown (reduced by upgrades)
    public float teleportDistance = 6f;     // teleport length
    private float teleportTimer = 0f;       // cooldown timer

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
        HandleTeleport();
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

        // ---------------- LANDING ----------------
        if (isGrounded && velocity.y < 0)
        {
            if (isFalling && landSFX != null)
                sfxSource.PlayOneShot(landSFX);

            velocity.y = -2f;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);

            isFalling = false;
            hasDoubleJumped = false;   // reset double jump
        }

        // ---------------- NORMAL JUMP ----------------
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = jumpForce;

            if (jumpSFX != null)
                sfxSource.PlayOneShot(jumpSFX);

            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);

            return;
        }

        // ---------------- DOUBLE JUMP (NO COOLDOWN) ----------------
        if (!isGrounded &&
            Input.GetKeyDown(KeyCode.Space) &&
            canDoubleJump &&        // ability unlocked
            !hasDoubleJumped)       // only once per jump
        {
            velocity.y = jumpForce;

            if (jumpSFX != null)
                sfxSource.PlayOneShot(jumpSFX);

            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);

            hasDoubleJumped = true;
        }

        // ---------------- FALLING ----------------
        if (!isGrounded && velocity.y < 0)
        {
            isFalling = true;
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
        }
    }

    private void HandleTeleport()
    {
        if (!canTeleport)
            return; // ability not unlocked yet

        // Cooldown countdown
        if (teleportTimer > 0f)
        {
            teleportTimer -= Time.deltaTime;
            return;
        }

        // Press E to teleport
        if (Input.GetKeyDown(KeyCode.T))
        {
            teleportTimer = teleportCooldown;

            // ✔ camera direction (FLAT, no Y)
            Vector3 camForward = cameraScript.GetCameraForwardFlat();

            // Desired target
            Vector3 targetPos = transform.position + camForward * teleportDistance;

            // Prevent teleport into walls
            if (Physics.Raycast(transform.position, camForward, out RaycastHit hit, teleportDistance))
            {
                targetPos = hit.point - camForward * 1f; // step back 1m
            }

            // Teleport safely
            controller.enabled = false;
            transform.position = targetPos;
            controller.enabled = true;

            // Play optional SFX/animation here:
            // sfxSource.PlayOneShot(teleportSFX);
            // animator.SetTrigger("Teleport");

            Debug.Log("Teleported toward CAMERA direction!");
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
