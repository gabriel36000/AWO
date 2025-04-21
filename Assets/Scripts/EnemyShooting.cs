using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Vector3 bulletoffset = new Vector3(0, 0.5f, 0);
    public Vector3 rotationoffset = new Vector3(0, 0, 0);

    public GameObject bulletPrefab;
    public float attackRange;
    public float fireDelay = 0.50f;
    private float cooldownTimer = 0;
    public AudioClip laserSound;

    private Transform target; // Now stores the nearest target
    public Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if (enemy.IsRetreating) return; // Stop shooting if retreating

        target = GetClosestTarget(); // Find closest target (Player or Friendly AI)

        if (target != null && Vector2.Distance(target.position, transform.position) <= attackRange)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                cooldownTimer = fireDelay;
                ShootAtTarget(target);
            }
        }
    }

    Transform GetClosestTarget()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject[] friendlies = GameObject.FindGameObjectsWithTag("Friendly");

        Transform closest = null;
        float minDistance = Mathf.Infinity;

        if (player != null)
        {
            float playerDistance = Vector2.Distance(transform.position, player.transform.position);
            if (playerDistance < minDistance)
            {
                minDistance = playerDistance;
                closest = player.transform;
            }
        }

        foreach (GameObject friendly in friendlies)
        {
            if (friendly != null)
            {
                float friendlyDistance = Vector2.Distance(transform.position, friendly.transform.position);
                if (friendlyDistance < minDistance)
                {
                    minDistance = friendlyDistance;
                    closest = friendly.transform;
                }
            }
        }

        return closest;
    }

    void ShootAtTarget(Transform target)
    {
        Vector3 offset = transform.rotation * bulletoffset;
        GameObject bulletGo = Instantiate(bulletPrefab, transform.position + offset, Quaternion.Euler(rotationoffset) * transform.rotation);
        if (IsVisibleToCamera())
        {
            AudioSource.PlayClipAtPoint(laserSound, transform.position);
        }
        // Set the Enemy reference and damage on the laser
        LaserDamageEnemy laserScript = bulletGo.GetComponent<LaserDamageEnemy>();
        if (laserScript != null)
        {
            laserScript.Enemy = enemy; // this is the enemy that fired the laser
            laserScript.damage = enemy.bulletDamage;
        }

        Destroy(bulletGo, 4f);
       
    }
    bool IsVisibleToCamera()
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
               viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
               viewportPoint.z > 0;
    }
}