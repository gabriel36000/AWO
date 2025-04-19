using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Friendly : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float stoppingDistance;
    public float rotationSpeed;
    public int minDamage;
    public int maxDamage;
    public int damage;
    public int criticalChance;
    private Transform target;
    public int currentHealth;
    public int maxHealth;
    public int xpValue;
    public GameObject explosion;
    public float gotoRange;
    private Rigidbody2D rigidbody;
    private Vector2 patrolTarget;
    private bool hasStartedPatrolling = false;
    private bool isRetreating = false;
    private float safeDistance = 100f;
    public int healRate = 1;
    public float healInterval = 1f;
    private float healTimer = 0f;
    public VolumeMaker volumeMaker;
    public AudioClip explosionSound;

    void Start()
    {
        currentHealth = maxHealth;
        rigidbody = GetComponent<Rigidbody2D>();
        SetRandomPatrolTarget();
    }

    void Update()
    {
        FindClosestEnemy();
        damage = Random.Range(minDamage, maxDamage);

        if (currentHealth >= maxHealth * 0.25f)
        {
            if (target != null && Vector2.Distance(target.position, transform.position) <= gotoRange)
            {
                go();
            }
            else
            {
                randomPatrol();
            }
        }
        else
        {
            RetreatAndHeal();
        }

        death();
    }

    private void RotateTowards(Vector2 targetPosition)
    {
        var offset = 270f;
        Vector2 direction = targetPosition - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private void go()
    {
        float distance = Vector2.Distance(target.position, transform.position);
        RotateTowards(target.position);

        if (distance > stoppingDistance)
        {
            speed = Mathf.Lerp(speed, maxSpeed, 0.01f);
            rigidbody.velocity = transform.up * speed;
        }
        else
        {
            speed = Mathf.Lerp(speed, 0f, 0.015f);
            rigidbody.velocity = transform.up * speed;
        }
    }

    private void RetreatAndHeal()
    {
        if (target == null) return;

        float distanceToEnemy = Vector2.Distance(transform.position, target.position);

        if (!isRetreating)
        {
            isRetreating = true;
        }

        if (distanceToEnemy < safeDistance)
        {
            Vector2 retreatDirection = (transform.position - target.position).normalized;
            float targetAngle = Mathf.Atan2(retreatDirection.y, retreatDirection.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, targetAngle);
            rigidbody.velocity = retreatDirection * speed;
            speed = Mathf.Lerp(speed, maxSpeed, 0.02f);
        }
        else
        {
            speed = Mathf.Lerp(speed, 0f, 0.01f);
            rigidbody.velocity = transform.up * speed;
            if (speed <= 0.1f)
            {
                isRetreating = false;
            }
        }

        // Healing during retreat
        if (currentHealth < maxHealth)
        {
            healTimer += Time.deltaTime;
            if (healTimer >= healInterval)
            {
                currentHealth = Mathf.Min(currentHealth + healRate, maxHealth);
                healTimer = 0f;
            }
        }
    }

    private void SetRandomPatrolTarget()
    {
        float minX = -607.8f, maxX = -322.2f;
        float minY = 168.6f, maxY = 337.9f;
        float randomX = UnityEngine.Random.Range(minX, maxX);
        float randomY = UnityEngine.Random.Range(minY, maxY);
        patrolTarget = new Vector2(randomX, randomY);
    }

    private void randomPatrol()
    {
        if (Vector2.Distance(transform.position, patrolTarget) <= 1f)
        {
            SetRandomPatrolTarget();
        }

        RotateTowards(patrolTarget);
        transform.position = Vector2.MoveTowards(transform.position, patrolTarget, speed * Time.deltaTime);
        rigidbody.velocity = transform.up * speed;
        speed = Mathf.Lerp(speed, maxSpeed, 0.01f);
    }

    private void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        List<GameObject> allEnemies = new List<GameObject>(enemies);
        allEnemies.AddRange(bosses);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in allEnemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }
        target = closestEnemy;
    }

    private void death()
    {
        if (currentHealth <= 0)
        {
            VolumeMaker.Play2DSoundIfCloseToCamera(explosionSound, transform.position, 20f, 0.3f);
            explosion.SetActive(true);
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}