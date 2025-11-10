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
    [SerializeField] private float fireRateDelay = 0.15f;
    [SerializeField] private int multishot = 1;
    [SerializeField] private float spreadAngle = 10f;
    [SerializeField] private float spawnOffset = 0.3f;
    [SerializeField] private float bulletLifetime = 5f;

    private bool canShoot = true;

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
            playerManager.currentState == playerManager.aimState;

        if (canShootNow && Input.GetMouseButtonDown(0) && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(1000f);

        Vector3 shootDir = (targetPoint - firingPoint.position).normalized;

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

        yield return new WaitForSeconds(fireRateDelay);
        canShoot = true;
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
