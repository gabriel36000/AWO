using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;


public class FriendlyTurret : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public int minDamage;
    public int maxDamage;
    public int damage;
    public int criticalChance;
    public int currentHealth;
    public int maxHealth;
    public GameObject explosion;
    public Image friendlyHealth;
    public int healRate = 1;
    public float healInterval = 1f;
    public AudioClip explosionSound;
    public VolumeMaker volumeMaker;
    public AIShooter iShooter;
    private Transform target;
    private float healTimer = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        iShooter = GetComponent<AIShooter>();
    }

    void Update()
    {
        // Update health bar UI
        if (friendlyHealth != null)
            friendlyHealth.fillAmount = (float)currentHealth / maxHealth;

        // Damage is randomized every frame (you may want to randomize only when firing)
        damage = Random.Range(minDamage, maxDamage);

        // Healing over time
        Heal();

        // Find and engage the closest enemy
        FindClosestEnemy();

        if (currentHealth >= maxHealth * 0.25f && target != null)
        {
            float distance = Vector2.Distance(target.position, transform.position);

            // Optional range check if you only want to rotate when enemy is in a certain range
            if (distance <= iShooter.attackRange) // Adjust this range as needed
            {
                RotateTowards(target.position);
                // You can call your shooting logic here if needed
            }
        }

        CheckDeath();
    }

    private void RotateTowards(Vector2 targetPosition)
    {
        float offset = 270f;
        Vector2 direction = targetPosition - (Vector2)transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
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

    private void Heal()
    {
        healTimer += Time.deltaTime;
        if (healTimer >= healInterval && currentHealth < maxHealth)
        {
            currentHealth += healRate;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            healTimer = 0f;
        }
    }

    private void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            if (explosionSound != null && volumeMaker != null)
            {
                VolumeMaker.Play2DSoundIfCloseToCamera(explosionSound, transform.position, 20f, 0.3f);
            }

            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}

