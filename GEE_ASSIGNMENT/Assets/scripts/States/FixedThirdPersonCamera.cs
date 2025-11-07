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

    private float yaw;
    private float pitch;
    private Vector3 currentVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = target.eulerAngles.y;
        pitch = 10f;
    }

    void LateUpdate()
    {
        if (!target) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Build rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Keep a constant distance behind target (without pitching the offset)
        Vector3 cameraOffset = rotation * Vector3.back * distance;
        Vector3 desiredPos = target.position + Vector3.up * height + cameraOffset;

        // Smooth movement
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref currentVelocity, smoothTime);
        transform.LookAt(target.position + Vector3.up * height);
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
