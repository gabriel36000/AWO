using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooter : MonoBehaviour
{

    public GameObject[] enemies; // Array to store all enemies in the scene
    public GameObject boss;      // Reference to the boss
    public GameObject bulletPrefab; // Bullet prefab to instantiate
    public float attackRange = 10f; // Range to start shooting at targets
    public float fireDelay = 0.5f;  // Delay between shots
    private float cooldownTimer = 0f; // Cooldown timer
    public AudioClip laserSound;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Find all enemies in the scene
        boss = GameObject.FindGameObjectWithTag("Boss");      // Find the boss object
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Update enemies list
        boss = GameObject.FindGameObjectWithTag("Boss");      // Update boss reference

        GameObject target = GetTarget();

        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0)
            {
                ShootAtTarget(target);
                cooldownTimer = fireDelay; // Reset cooldown timer
            }
        }
    }

    // Find the nearest target (either an enemy or the boss)
    public GameObject GetTarget()
    {
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity; // Start with the maximum possible distance

        // Check distance to all enemies
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null) // Ensure the enemy is valid (it might have been destroyed)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = enemy;
                }
            }
        }

        // If there's a boss and it's closer than any enemy, target the boss instead
        if (boss != null)
        {
            float distanceToBoss = Vector2.Distance(transform.position, boss.transform.position);
            if (distanceToBoss < closestDistance)
            {
                closestTarget = boss;
            }
        }

        return closestTarget;
    }

    // Shoot at the target (instantiates the bullet)
    public void ShootAtTarget(GameObject target)
    {
        Vector3 shootingDirection = (target.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, shootingDirection); // Rotate towards target

        // Instantiate the bullet
        GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, rotation);
        if (IsVisibleToCamera())
        {
            AudioSource.PlayClipAtPoint(laserSound, transform.position);
        }
        // Destroy the instantiated bullet after 4 seconds
        Destroy(bulletInstance, 4f);
    }
    bool IsVisibleToCamera()
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
               viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
               viewportPoint.z > 0;
    }
}