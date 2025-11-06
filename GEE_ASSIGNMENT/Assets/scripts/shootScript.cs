using System.Collections;
using UnityEngine;

public class shootScript : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float impulseStrength = 5.0f;
    [SerializeField] private Transform firingpoint;
    [SerializeField] private float fireRateDelay = 0.2f;
    [SerializeField] private int multishot = 1;
    [SerializeField] private float spreadAngle = 15f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform gun;
    [SerializeField] private float maxAimDistance = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GameObject.FindAnyObjectByType<Camera>();
    }
    void setfireRatedelay(float newdelay)
    {
        fireRateDelay = newdelay;
    }

    void AimGunAtCenter()
    {
        // Just aim straight forward from the camera — no raycast
        Vector3 aimPoint = cam.transform.position + cam.transform.forward * 1000f;

        // Make the gun and firing point look in that direction
        gun.LookAt(aimPoint);
        firingpoint.LookAt(aimPoint);
    }

    // Update is called once per frame
    void Update()
    {
        //gun.rotation = Quaternion.LookRotation(cam.transform.forward);
        //firingpoint.rotation = Quaternion.LookRotation(cam.transform.forward);
        AimGunAtCenter();

        Debug.Log("gun is active");
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("shooting");
            StartCoroutine(Shoot());
        }
    }
    IEnumerator Shoot()
    {
        if (multishot == 1)
        {
            FireProjectile(firingpoint.rotation);
            yield return new WaitForSeconds(fireRateDelay);
        }
        else
        {

            float startAngle = -spreadAngle * 0.5f;
            float angleStep = spreadAngle / (multishot - 1);

            for (int i = 0; i < multishot; i++)
            {
                // Calculate rotation offset
                Quaternion shotRotation = firingpoint.rotation * Quaternion.Euler(0, startAngle + (angleStep * i), 0);
                FireProjectile(shotRotation);
            }
            yield return new WaitForSeconds(fireRateDelay);
        }
        void FireProjectile(Quaternion rotation)
        {
            GameObject projectile = Instantiate(projectilePrefab, firingpoint.position, rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(rotation * Vector3.forward * impulseStrength, ForceMode.Impulse);
            }

            Destroy(projectile, 5f); // optional cleanup
        }
    }
}