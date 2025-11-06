using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class animationStateController : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;

    [Header("Movement Speeds")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float backSpeed = 4f; // backward running speed
    public float gravity = -9.8f;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool wPressed = Input.GetKey(KeyCode.W);
        bool sPressed = Input.GetKey(KeyCode.S);
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift);

        Vector3 move = Vector3.zero;

        // FORWARD MOVEMENT
        if (wPressed && shiftPressed)
        {
            // Run forward
            move = transform.forward * runSpeed;
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunningBackward", false);
        }
        else if (wPressed && !shiftPressed)
        {
            // Walk forward
            move = transform.forward * walkSpeed;
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isRunningBackward", false);
        }
        // BACKWARD MOVEMENT (always running)
        else if (sPressed)
        {
            move = -transform.forward * backSpeed;
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isRunningBackward", true);
        }
        // IDLE
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isRunningBackward", false);
        }

        // Apply gravity and move
        move.y = gravity;
        controller.Move(move * Time.deltaTime);
    }
}
