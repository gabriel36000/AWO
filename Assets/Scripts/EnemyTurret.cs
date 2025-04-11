using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTurret : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float attackRange = 10f;
    public GameObject bulletPrefab;
    public Vector3 bulletOffset = new Vector3(0, 0.5f, 0);
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public float fireDelay = 0.5f;
    public AudioClip laserSound;
    public int currentDamage = 10;

    private float cooldownTimer = 0f;
    private Transform target;

    void Update()
    {
        target = GetClosestTarget();

        if (target != null)
        {
            RotateTowards(target.position);

            if (Vector2.Distance(transform.position, target.position) <= attackRange)
            {
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    cooldownTimer = fireDelay;
                    ShootAtTarget();
                }
            }
        }
    }

    void RotateTowards(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.Euler(0, 0, angle - 90f); // Adjust for sprite facing up
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
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

    void ShootAtTarget()
    {
        Vector3 offset = transform.rotation * bulletOffset;
        GameObject bulletGo = Instantiate(bulletPrefab, transform.position + offset, Quaternion.Euler(rotationOffset) * transform.rotation);

        if (IsVisibleToCamera() && laserSound != null)
        {
            AudioSource.PlayClipAtPoint(laserSound, transform.position);
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
