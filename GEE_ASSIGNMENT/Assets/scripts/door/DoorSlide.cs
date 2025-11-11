using UnityEngine;

public class DoorSlide : MonoBehaviour
{
    public float speed = 1.0f;
    private bool isSliding = false;


    public Vector3 closedPosition;
    public Vector3 openPosition;

    private float interpolate = 0.0f; // between 0 (closed) and 1 (open)
    private int direction = 1; // 1 = opening, -1 = closing


    void Update()
    {
        if (isSliding)
        {
            interpolate += direction * speed * Time.deltaTime;

            // Clamp the interpolate value between 0 and 1
            interpolate = Mathf.Clamp01(interpolate);

            // Lerp between the closed and open positions
            transform.localPosition = Vector3.Lerp(closedPosition, openPosition, interpolate);

            // Stop when fully opened or closed
            if (interpolate == 0f || interpolate == 1f)
                isSliding = false;
        }
    }

    public void Activate()
    {
        // Start opening
        direction = 1;
        isSliding = true;
    }

    public void Deactivate()
    {
        // Start closing
        direction = -1;
        isSliding = true;
    }
}