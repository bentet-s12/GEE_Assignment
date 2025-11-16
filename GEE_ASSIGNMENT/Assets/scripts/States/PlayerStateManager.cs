using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerStateManager : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public FixedThirdPersonCamera cameraScript;
    [HideInInspector] public CharacterController controller;

    [Header("Audio")]
    public AudioSource sfxSource;
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
    public bool canTeleport = false;
    public float teleportCooldown = 6f;
    public float teleportDistance = 6f;
    private float teleportTimer = 0f;

    // States
    [HideInInspector] public PlayerState currentState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public WalkState walkState;
    [HideInInspector] public RunState runState;
    [HideInInspector] public AimState aimState;
    [HideInInspector] public JumpState jumpState;

    [HideInInspector] public bool isAiming = false;
    [HideInInspector] public bool isDead = false;

    [SerializeField] private GameObject DeathUI;
    [SerializeField] private GameObject Parent_player;
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
        if (isDead) return;

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
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = !isAiming;

            animator.SetBool("Aiming", isAiming);
            cameraScript.SetAiming(isAiming);
        }
    }

    // ------------------ MOVEMENT ------------------
    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        animator.SetFloat("hzinput", h);
        animator.SetFloat("vinput", v);

        Vector3 forward = cameraScript.GetCameraForwardFlat();
        Vector3 right = cameraScript.GetCameraRightFlat();
        Vector3 move = forward * v + right * h;

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (isAiming)
        {
            Vector3 camDir = cameraScript.GetCameraForwardFlat();
            if (camDir != Vector3.zero)
            {
                Quaternion aimRot = Quaternion.LookRotation(camDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, aimRot, Time.deltaTime * 15f);
            }

            Vector3 strafeMove = (forward * v) + (right * h);
            if (strafeMove.sqrMagnitude > 0.0001f)
            {
                controller.Move(strafeMove.normalized * speed * Time.deltaTime);
            }

            return;
        }

        if (move.magnitude > 0.1f)
        {
            controller.Move(move.normalized * speed * Time.deltaTime);

            Vector3 lookDir = cameraScript.GetCameraForwardFlat();
            if (lookDir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 15f);
            }
        }
    }

    // ------------------ JUMP ------------------
    private void HandleJumpInput()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            if (isFalling && landSFX != null)
                sfxSource.PlayOneShot(landSFX);

            velocity.y = -2f;

            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);

            isFalling = false;
            hasDoubleJumped = false;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = jumpForce;

            if (jumpSFX != null)
                sfxSource.PlayOneShot(jumpSFX);

            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
            return;
        }

        // Double jump
        if (!isGrounded &&
            Input.GetKeyDown(KeyCode.Space) &&
            canDoubleJump &&
            !hasDoubleJumped)
        {
            velocity.y = jumpForce;

            if (jumpSFX != null)
                sfxSource.PlayOneShot(jumpSFX);

            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);

            hasDoubleJumped = true;
        }

        // Falling
        if (!isGrounded && velocity.y < 0)
        {
            isFalling = true;
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
        }
    }

    private void HandleTeleport()
    {
        if (!canTeleport) return;

        if (teleportTimer > 0f)
        {
            teleportTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            teleportTimer = teleportCooldown;

            Vector3 camForward = cameraScript.GetCameraForwardFlat();
            Vector3 targetPos = transform.position + camForward * teleportDistance;

            if (Physics.Raycast(transform.position, camForward, out RaycastHit hit, teleportDistance))
            {
                targetPos = hit.point - camForward * 1f;
            }

            controller.enabled = false;
            transform.position = targetPos;
            controller.enabled = true;

            Debug.Log("Teleported toward CAMERA direction!");
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        controller.enabled = false;
        animator.applyRootMotion = true;

        animator.SetTrigger("Die");
        animator.SetBool("Walking", false);
        animator.SetBool("Running", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("Aiming", false);

        cameraScript.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        shootScript gun = FindFirstObjectByType<shootScript>();
        if (gun != null)
        {

            gun.enabled = false;
        }

        DeathUI.SetActive(true);
        Parent_player.GetComponent<DDL>().enabled = false;
        Debug.Log("PLAYER DIED");
        
    }

    // ------------------ GRAVITY ------------------
    private void ApplyGravity()
    {
        if (isDead) return;

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
