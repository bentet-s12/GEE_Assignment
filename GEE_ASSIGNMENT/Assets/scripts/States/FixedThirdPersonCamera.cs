using UnityEngine;

public class FixedThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 3.5f;
    [SerializeField] private float height = 2f;

    [Header("Rotation Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float minPitch = -40f;
    [SerializeField] private float maxPitch = 60f;
    [SerializeField] private float smoothTime = 0.1f;

    [Header("Zoom Settings")]
    [SerializeField] private float normalDistance = 3.5f;   // default camera distance
    [SerializeField] private float aimDistance = 2.0f;      // closer when aiming
    [SerializeField] private float zoomSpeed = 5f;          // how fast to zoom

    private float yaw;
    private float pitch;
    private Vector3 currentVelocity;
    private bool isAiming = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = target.eulerAngles.y;
        pitch = 10f;

        // start with default distance
        distance = normalDistance;
    }

    void LateUpdate()
    {
        if (!target) return;

        // mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // smoothly adjust zoom distance
        float targetDistance = isAiming ? aimDistance : normalDistance;
        distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * zoomSpeed);

        // build rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // compute camera offset (keep above target by height)
        Vector3 cameraOffset = rotation * Vector3.back * distance;
        Vector3 desiredPos = target.position + Vector3.up * height + cameraOffset;

        // smooth position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref currentVelocity, smoothTime);

        // look at player
        transform.LookAt(target.position + Vector3.up * height);
    }

    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
    }

    public Vector3 GetCameraForwardFlat()
    {
        Vector3 fwd = transform.forward;
        fwd.y = 0;
        return fwd.normalized;
    }

    public Vector3 GetCameraRightFlat()
    {
        Vector3 right = transform.right;
        right.y = 0;
        return right.normalized;
    }
}
