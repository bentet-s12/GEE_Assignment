using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // enforces dependency on character controller
[AddComponentMenu("Control Script/FPS Input")]  // add to the Unity editor's component menu
public class FPSInput : MonoBehaviour
{
    // movement sensitivity
    public float speed = 6.0f;

    // gravity setting
    public float gravity = -9.8f;
    public float jumpHeight = 8.0f;
    public int jumpAmount = 2;

    // reference to the character controller
    private CharacterController charController;
    private float verticalVelocity;
    private int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        // get the character controller component
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // changes based on WASD keys
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        // make diagonal movement consistent
        movement = Vector3.ClampMagnitude(movement, speed);

        //ground check
        if (charController.isGrounded) 
        {
            jumpCount = jumpAmount;
            verticalVelocity = -1f;
        }

        if(Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            verticalVelocity = jumpHeight;
            jumpCount--;
        }

        verticalVelocity += gravity * Time.deltaTime;

        // add gravity in the vertical direction
        movement.y = verticalVelocity;

        // ensure movement is independent of the framerate
        movement *= Time.deltaTime;

        // transform from local space to global space
        movement = transform.TransformDirection(movement);

        // pass the movement to the character controller
        charController.Move(movement);
    }
}
