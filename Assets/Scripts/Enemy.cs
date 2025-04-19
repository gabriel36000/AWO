using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float stoppingDistance;
    public float rotationSpeed;
    private Transform target;
    public GameObject player;
    public int currentHealth;
    public int maxHealth;
    public int xpValue;
    LevelSystem levelSystem;            //store scripts 
    public GameObject explosion;
    public TextMeshPro enemyName;
    public Image enemyHealth;
    public int bulletDamage;
    public float gotoRange;
    public Transform player1;
    Rigidbody2D rigidbody;
    Animator damageAnim;
    Player player2;
    public int moneyValueLow, moneyValueHigh;
    PlayerMoney money;
    public GameObject popUpPreFabMoney;
    public bool isRetreating = false;
    private float healTimer = 0f;
    public float healRate = 0.01f;
    public float safeDistance = 100f;
    private Vector2 patrolTarget;
    public bool isMinion;
    public Transform bossTransform;
    private bool hasStartedPatrolling = false; // Prevents minions from using randomPatrol first
    public bool IsRetreating => isRetreating;
    private Transform friendlyTarget;
    private Rigidbody2D rb;
    public VolumeMaker volumeMaker;
    public AudioClip explosionSound;





    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        InitializePlayerReferences();

        // Find the boss dynamically if this enemy is a minion
        if (isMinion)
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            if (boss != null)
            {
                bossTransform = boss.transform;
                SetInitialMinionPatrolTarget(); // Set the first patrol point around the boss
            }
            else
            {
                Debug.LogWarning("Boss not found! Make sure the boss has the 'Boss' tag.");
            }
        }
        else
        {
            SetRandomPatrolTarget();
        }



    }

    // Update is called once per frame
    void Update()
    {
        // Attempt to reassign player reference if it's missing or null
        if (player == null)
        {
            InitializePlayerReferences();
        }

        // If player is still missing or inactive, default to patrol behavior
        if (player == null || !player.activeInHierarchy || player1 == null)
        {
            target = null;

            if (isMinion && bossTransform != null)
            {
                PatrolAroundBoss();
            }
            else
            {
                randomPatrol();
            }

            return;
        }

        // Update health bar UI
        if (enemyHealth != null)
        {
            enemyHealth.fillAmount = (float)currentHealth / maxHealth;
        }

        // Find nearest friendly unit (if any)
        FindClosestFriendly();

        // Behavior based on health
        if (currentHealth >= maxHealth * 0.25f)
        {
            float playerDistance = Vector2.Distance(player1.position, transform.position);
            float friendlyDistance = (friendlyTarget != null) ? Vector2.Distance(friendlyTarget.position, transform.position) : Mathf.Infinity;

            if (playerDistance <= gotoRange)
            {
                target = player1;
                go();
            }
            else if (friendlyTarget != null && friendlyDistance <= gotoRange)
            {
                target = friendlyTarget;
                go();
            }
            else
            {
                randomPatrol();
            }
        }
        else
        {
            Retreat();
        }

        // Check for death
        death();
    }
    private void RotateTowards(Vector2 target)
    {
        var offset = 270f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private void death()
    {
            if (currentHealth <= 0)
            {
                if (IsVisibleToCamera())
                {
                    levelSystem.currentXP += xpValue;
                    levelSystem.totalXP += xpValue;
                    int earned = UnityEngine.Random.Range(moneyValueLow, moneyValueHigh);
                    money.AddMoney(earned);
                    GameObject PopUpmoney = Instantiate(popUpPreFabMoney, transform.position, Quaternion.identity);
                    PopUpmoney.transform.GetChild(0).GetComponent<TextMeshPro>().text = "money: " + earned.ToString();

                    Destroy(PopUpmoney, 2.7f);
                    explosion.SetActive(true);
                    Instantiate(explosion, transform.position, transform.rotation);
                    Destroy(gameObject);
                }
                VolumeMaker.Play2DSoundIfCloseToCamera(explosionSound, transform.position, 20f, 0.3f);
                explosion.SetActive(true);
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject);



            }
        }
    private void go()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is null, cannot execute go() movement.");
            return;
        }

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

        rigidbody.interpolation = RigidbodyInterpolation2D.None;
    }

    private void Retreat()
    {
        // Reacquire the player reference if missing or destroyed
        if (player1 == null || !player1)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player1 = playerObj.transform;
            }
            else
            {
                isRetreating = false;
                rigidbody.velocity = Vector2.zero; // stop movement
                return;
            }
        }

        // If player is inactive, stop retreating
        if (!player1.gameObject.activeInHierarchy)
        {
            isRetreating = false;
            rigidbody.velocity = Vector2.zero; // stop movement
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player1.position);
        float distanceToFriendly = (friendlyTarget != null && friendlyTarget) ? Vector2.Distance(transform.position, friendlyTarget.position) : Mathf.Infinity;

        if (currentHealth < maxHealth * 0.25f && !isRetreating)
        {
            isRetreating = true;
            print("Enemy health is below 25%, retreating...");
        }

        if (isRetreating)
        {
            if (distanceToPlayer < safeDistance || distanceToFriendly < safeDistance)
            {
                // Calculate direction away from both player and friendly AI
                Vector2 retreatDirection = (Vector2)transform.position - (Vector2)player1.position;
                if (friendlyTarget != null && friendlyTarget)
                {
                    retreatDirection += (Vector2)transform.position - (Vector2)friendlyTarget.position;
                }

                retreatDirection.Normalize();

                // Set rotation to move away
                float targetAngle = Mathf.Atan2(retreatDirection.y, retreatDirection.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.Euler(0, 0, targetAngle);

                // Move in the retreat direction
                rigidbody.velocity = retreatDirection * speed;
                speed = Mathf.Lerp(speed, maxSpeed, 0.02f);
            }
            else
            {
                // Decelerate smoothly
                speed = Mathf.Lerp(speed, 0f, 0.01f);
                rigidbody.velocity = transform.up * speed;

                // Heal over time
                healTimer += Time.deltaTime;
                if (healTimer >= 1f)
                {
                    currentHealth += (int)healRate;
                    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                    healTimer = 0f;
                    print("Healing... Current HP: " + currentHealth);
                }

                // Return to normal behavior when healed enough
                if (currentHealth >= maxHealth * 0.25f)
                {
                    isRetreating = false;
                    print("Healed enough! Returning to normal behavior.");
                }
            }
        }
    }
    private void SetRandomPatrolTarget()
    {
        // Define the map boundaries
        float minX = -607.8f;  // Left
        float maxX = -322.2f;  // Right
        float minY = 168.6f;   // Bottom
        float maxY = 337.9f;   // Top

        // Set a random position within the map bounds
        float randomX = UnityEngine.Random.Range(minX, maxX);  // X-axis within bounds
        float randomY = UnityEngine.Random.Range(minY, maxY);  // Y-axis within bounds
        patrolTarget = new Vector2(randomX, randomY);

        // Debug: Print the patrol target coordinates
        Debug.Log("New Patrol Target: " + patrolTarget);
    }

    private void randomPatrol()
    {
        if (isMinion && bossTransform != null)
        {
            PatrolAroundBoss();
        }
        else
        {
            if (Vector2.Distance(transform.position, patrolTarget) <= 1f)
            {
                SetRandomPatrolTarget();
            }

            RotateTowards(patrolTarget);
            transform.position = Vector2.MoveTowards(transform.position, patrolTarget, speed * Time.deltaTime);  // Move towards patrol target
            rb.velocity = transform.up * speed;  // Ensure movement is applied correctly
            speed = Mathf.Lerp(speed, maxSpeed, 0.01f);
        }
    }
    private void PatrolAroundBoss()
    {
        if (bossTransform == null || bossTransform.gameObject == null)
            return;

        float patrolRadius = 10f;

        float minX = bossTransform.position.x - patrolRadius;
        float maxX = bossTransform.position.x + patrolRadius;
        float minY = bossTransform.position.y - patrolRadius;
        float maxY = bossTransform.position.y + patrolRadius;

        if (Vector2.Distance(transform.position, patrolTarget) <= 1f)
        {
            patrolTarget = new Vector2(
                UnityEngine.Random.Range(minX, maxX),
                UnityEngine.Random.Range(minY, maxY)
            );
            Debug.Log($"Minion Patrol Target: {patrolTarget}, Boss Position: {bossTransform.position}");
        }

        RotateTowards(patrolTarget);
        transform.position = Vector2.MoveTowards(transform.position, patrolTarget, speed * Time.deltaTime);
        speed = Mathf.Lerp(speed, maxSpeed, 0.02f);

        // Use rb instead of rigidbody to avoid unassigned errors
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }
    private void SetInitialMinionPatrolTarget()
    {
        if (bossTransform == null) return;

        float patrolRadius = 20f; // Distance from boss

        // Pick a random point around the boss
        float randomAngle = UnityEngine.Random.Range(0f, 360f);
        float xOffset = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * patrolRadius;
        float yOffset = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * patrolRadius;

        patrolTarget = new Vector2(bossTransform.position.x + xOffset, bossTransform.position.y + yOffset);

        Debug.Log("Initial minion patrol target: " + patrolTarget);
    }
    private void SetNewBossPatrolTarget()
    {
        if (bossTransform == null) return;

        float patrolRadius = 10f;  // Patrol area stays within 10 units of the boss
        float randomAngle = UnityEngine.Random.Range(0f, 360f);  // Get a random angle around the boss
        float xOffset = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * patrolRadius;
        float yOffset = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * patrolRadius;

        patrolTarget = new Vector2(bossTransform.position.x + xOffset, bossTransform.position.y + yOffset);

        Debug.Log("New Minion Patrol Target (Around Boss): " + patrolTarget);
    }
    private void FindClosestFriendly()
    {
        GameObject[] friendlies = GameObject.FindGameObjectsWithTag("Friendly");
        float closestDistance = Mathf.Infinity;
        Transform closestFriendly = null;

        foreach (GameObject friendly in friendlies)
        {
            float distance = Vector2.Distance(transform.position, friendly.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestFriendly = friendly.transform;
            }
        }

        friendlyTarget = closestFriendly;
    }
    private void InitializePlayerReferences()
    {
        player = GameObject.FindWithTag("Player");

        if (player == null || !player.activeInHierarchy)
        {
            Debug.LogWarning("Player not found or inactive. Skipping initialization.");
            return;
        }

        // Safely assign references
        player1 = player.transform;
        target = player.transform;

        levelSystem = player.GetComponent<LevelSystem>();
        if (levelSystem == null)
        {
            Debug.LogWarning("LevelSystem component not found on Player.");
        }

        money = player.GetComponent<PlayerMoney>();
        if (money == null)
        {
            Debug.LogWarning("PlayerMoney component not found on Player.");
        }

        rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody == null)
        {
            Debug.LogError("Rigidbody2D not found on Enemy.");
        }

        currentHealth = maxHealth;
    }
    bool IsVisibleToCamera()
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
               viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
               viewportPoint.z > 0;
    }
}