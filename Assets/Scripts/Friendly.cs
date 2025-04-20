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
    public Transform healthCenter;
    public GameObject healingTextGameObject;
    public bool isInHealingCenter = false;

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

        if (isRetreating)
        {
            RetreatAndHeal();

            // ✅ Exit healing only when fully healed AND inside health center
            if (currentHealth >= maxHealth && isInHealingCenter)
            {
                isRetreating = false;
            }
        }
        else if (currentHealth < maxHealth * 0.25f)
        {
            isRetreating = true;
            RetreatAndHeal();
        }
        else
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
        if (healthCenter == null) return;

        isRetreating = true;

        float distanceToCenter = Vector2.Distance(transform.position, healthCenter.position);

        // ✅ Heal regardless of whether you're in the health center or not
        healTimer += Time.deltaTime;
        if (healTimer >= healInterval && currentHealth < maxHealth)
        {
            int healAmount = Mathf.CeilToInt(maxHealth * 0.005f); // 0.5% heal rate
            currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
            healTimer = 0f;

            if (healingTextGameObject != null)
            {
                GameObject healPopUp = Instantiate(healingTextGameObject, transform.position, Quaternion.identity);
                healPopUp.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + healAmount;
                Destroy(healPopUp, 1f);
            }
        }

        if (distanceToCenter > 0.5f)
        {
            Vector2 directionToCenter = (healthCenter.position - transform.position).normalized;
            float targetAngle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, targetAngle);
            rigidbody.velocity = directionToCenter * speed;
            speed = Mathf.Lerp(speed, maxSpeed, 0.02f);
        }
        else
        {
            speed = Mathf.Lerp(speed, 0f, 0.01f);
            rigidbody.velocity = Vector2.zero;

            // ❌ Don't stop retreating yet — wait until fully healed
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HealthCenter"))
        {
            isInHealingCenter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("HealthCenter"))
        {
            isInHealingCenter = false;
        }
    }
}