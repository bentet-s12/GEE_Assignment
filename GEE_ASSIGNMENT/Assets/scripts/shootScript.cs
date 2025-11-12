using System.Collections;
using UnityEngine;

public class shootScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firingPoint;       // Gun muzzle
    [SerializeField] private GameObject projectilePrefab; // Bullet prefab
    [SerializeField] private Camera cam;                  // Main camera
    [SerializeField] private PlayerStateManager playerManager;

    [Header("Settings")]
    [SerializeField] private float bulletSpeed = 80f;
    [SerializeField] private float fireRateDelay = 0.1f;  // Lower = faster fire rate
    [SerializeField] private int multishot = 1;
    [SerializeField] private float spreadAngle = 10f;
    [SerializeField] private float spawnOffset = 0.3f;
    [SerializeField] private float bulletLifetime = 5f;

    private bool isShooting = false;
    private Coroutine shootingCoroutine;

    void Awake()
    {
        if (cam == null)
            cam = Camera.main;

        if (playerManager == null)
            playerManager = Object.FindFirstObjectByType<PlayerStateManager>();
    }

    void Update()
    {
        bool canShootNow =
            playerManager.currentState == playerManager.walkState ||
            playerManager.currentState == playerManager.runState ||
            playerManager.currentState == playerManager.aimState ||
            playerManager.currentState == playerManager.jumpState;
           

        // Start shooting when button is pressed down
        if (Input.GetMouseButtonDown(0) && canShootNow && !isShooting)
        {
            isShooting = true;
            shootingCoroutine = StartCoroutine(ShootContinuously());
        }

        // Stop shooting when button is released
        if (Input.GetMouseButtonUp(0) && isShooting)
        {
            isShooting = false;
            StopCoroutine(shootingCoroutine);
        }
    }

    IEnumerator ShootContinuously()
    {
        while (isShooting)
        {
            FireProjectileBurst();
            yield return new WaitForSeconds(fireRateDelay);
        }
    }

    void FireProjectileBurst()
    {
        // Ray from camera to center of screen
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(1000f);

        Vector3 shootDir = (targetPoint - firingPoint.position).normalized;

        // Handle single or multishot
        if (multishot <= 1)
        {
            FireProjectile(shootDir);
        }
        else
        {
            float startAngle = -spreadAngle * 0.5f;
            float angleStep = spreadAngle / (multishot - 1);

            for (int i = 0; i < multishot; i++)
            {
                Quaternion spreadRot = Quaternion.AngleAxis(startAngle + angleStep * i, Vector3.up);
                Vector3 spreadDir = spreadRot * shootDir;
                FireProjectile(spreadDir);
            }
        }
    }

    void FireProjectile(Vector3 direction)
    {
        Vector3 spawnPos = firingPoint.position + direction * spawnOffset;

        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(direction));

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        Destroy(projectile, bulletLifetime);
    }
}
