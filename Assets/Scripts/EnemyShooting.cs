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
        Destroy(bulletGo, 4f);
        Debug.Log("Enemy shooting at: " + target.name);
    }
}