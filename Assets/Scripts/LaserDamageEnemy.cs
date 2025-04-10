using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class LaserDamageEnemy : MonoBehaviour
{
    public int damage;
    private ShieldBarScript shield;
    private Player playerScript;
    public GameObject ShieldBarColor;
    public GameObject HealthBarColor;
    public GameObject damageEffect;
    public GameObject PopUpPreFab;
    public Enemy Enemy;

    void Start()
    {
        if (Enemy != null)
        {
            damage = Enemy.bulletDamage;
        }
        else
        {
            Debug.LogWarning("Enemy not assigned to laser!");
        }

        ShieldBarColor = GameObject.Find("ShieldBarColor");
        HealthBarColor = GameObject.Find("HealthBarColor");

        if (ShieldBarColor != null)
            shield = ShieldBarColor.GetComponent<ShieldBarScript>();

        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
            playerScript = playerObject.GetComponent<Player>();

        Destroy(gameObject, 3f); // Auto destroy after 3 seconds
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Friendly"))
        {
            if (col.CompareTag("Player"))
            {
                if (shield != null && shield.currentShield > 0)
                {
                    shield.currentShield -= damage;
                    shield.lastTime = Time.time;
                }
                else if (playerScript != null)
                {
                    playerScript.currentHealth -= damage;
                }
            }
            else if (col.CompareTag("Friendly"))
            {
                Friendly friendlyAI = col.GetComponent<Friendly>();
                if (friendlyAI != null)
                {
                    friendlyAI.currentHealth -= damage;
                }
            }

            if (damageEffect != null)
            {
                GameObject instance = Instantiate(damageEffect, transform.position, transform.rotation);
                Destroy(instance, 1f);
            }

            if (PopUpPreFab != null)
            {
                GameObject PopUpDamage = Instantiate(PopUpPreFab, transform.position, Quaternion.identity);
                PopUpDamage.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
                Destroy(PopUpDamage, 0.7f);
            }

            Destroy(gameObject); // Destroy the projectile on hit
        }
    }
}