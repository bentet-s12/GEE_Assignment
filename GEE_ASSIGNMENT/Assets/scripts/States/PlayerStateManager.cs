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
    public LayerMask groundMask;
    public float groundedOffset = 0.1f;

    private Vector3 velocity;
    private bool isGrounded;

    // States
    [HideInInspector] public PlayerState currentState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public WalkState walkState;
    [HideInInspector] public RunState runState;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        idleState = new IdleState(this);
        walkState = new WalkState(this);
        runState = new RunState(this);

        currentState = idleState;
        currentState.EnterState();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        currentState.UpdateState();
        ApplyGravity();
        HandleMovement();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Abs(h) < 0.1f && Mathf.Abs(v) < 0.1f) return;

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
        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y - controller.height / 2 + groundedOffset, transform.position.z);
        isGrounded = Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask);

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
