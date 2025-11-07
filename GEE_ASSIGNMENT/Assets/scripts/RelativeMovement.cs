using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    [SerializeField] Transform target;  // drag your Main Camera here
    public float moveSpeed = 6.0f;
    public float rotSpeed = 15.0f;

    private CharacterController charController;

    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {
            // camera-relative movement
            Vector3 right = target.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up);
            movement = (right * horInput) + (forward * vertInput);

            movement *= moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            // smoothly rotate player toward movement direction
            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

        // apply movement
        movement *= Time.deltaTime;
        charController.Move(movement);
    }
}
