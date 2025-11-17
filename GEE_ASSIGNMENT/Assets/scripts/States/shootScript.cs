using System.Collections;
using System.Linq.Expressions;
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
    [SerializeField] private int multishot = 0;
    [SerializeField] private float spreadAngle = 10f;
    [SerializeField] private float spawnOffset = 0.3f;
    [SerializeField] private float bulletLifetime = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int basedamage = 1;

    [Header("Gun Audio")]
    [SerializeField] private AudioSource gunAudioSource;
    [SerializeField] private AudioClip gunShotSFX;

    [HideInInspector] public bool isShooting = false;
    private Coroutine shootingCoroutine;

    void Awake()
    {
        if (cam == null)
            cam = Camera.main;

        if (playerManager == null)
            playerManager = Object.FindFirstObjectByType<PlayerStateManager>();
    }

    public int getdamage()
    {
        return damage;
    }
    public void setdmg(int dmg)
    {
        damage = dmg;
    }
    public int getmultishot()
    {
        return multishot;
    }

    void Update()
    {
        bool canShootNow =
     playerManager.isAiming ||
     playerManager.currentState == playerManager.walkState ||
     playerManager.currentState == playerManager.runState ||
     playerManager.currentState == playerManager.jumpState ||
     playerManager.isFalling;   

        // ---------------- START SHOOT (MODIFIED ONLY SOUND) ----------------
        if (Input.GetMouseButtonDown(0) && canShootNow && !isShooting)
        {
            isShooting = true;
            shootingCoroutine = StartCoroutine(ShootContinuously());
        }

        // ---------------- STOP SHOOT (MODIFIED ONLY SOUND) ----------------
        if (Input.GetMouseButtonUp(0) && isShooting)
        {
            isShooting = false;

            if (shootingCoroutine != null)
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
        if (gunAudioSource != null && gunShotSFX != null)
        {
            gunAudioSource.PlayOneShot(gunShotSFX);
        }

        // Ray from camera to center of screen
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Vector3 targetPoint = ray.GetPoint(1000f);
        Vector3 shootDir = (targetPoint - firingPoint.position).normalized;

        // Handle single or multishot
        if (multishot <= 0)
        {
            FireProjectile(shootDir);
        }
        else
        {
            float startAngle = -spreadAngle * 0.5f;
            float angleStep = spreadAngle / (multishot);

            for (int i = 0; i < multishot + 1; i++)
            {
                Quaternion spreadRot = Quaternion.AngleAxis(startAngle + angleStep * i, Vector3.up);
                Vector3 spreadDir = spreadRot * shootDir;
                FireProjectile(spreadDir);
            }
        }
    }

    public void setmultishot(int add)
    {
        multishot = add;
    }

    void FireProjectile(Vector3 direction)
    {
        Vector3 spawnPos = firingPoint.position + direction * spawnOffset;

        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(direction));
        bullet dmgScript = projectile.GetComponent<bullet>();
        if (dmgScript != null)
        {
            int currentdmg = dmgScript.getdmg();
            if (currentdmg != damage)
            {
                dmgScript.setdmg(currentdmg);
            }
        }
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        Destroy(projectile, bulletLifetime);
    }
}
