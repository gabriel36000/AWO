using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class LaserDamageEnemy : MonoBehaviour
{
    public int damage;
    private Player playerScript;
    public GameObject damageEffect;
    public GameObject PopUpPreFab;
    public Enemy Enemy;
    public EnemyTurret EnemyTurret;
    public AudioClip shieldHitSound;
    public VolumeMaker volumeMaker;
    public AudioClip healthHitSound;

    void Start()
    {
        if (Enemy != null)
        {
            damage = Enemy.bulletDamage;
        }
        else if (EnemyTurret != null)
        {
            damage = EnemyTurret.currentDamage;
        }
        else
        {
            Debug.LogWarning("Enemy not assigned to laser!");
        }

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
                if (playerScript.currentShield > 0)
                {
                    VolumeMaker.Play2DSoundIfCloseToCamera(shieldHitSound, transform.position, 20f, 0.5f);
                    playerScript.currentShield -= damage;
                    playerScript.lastShieldRegenTime = Time.time;
                }
                else if (playerScript != null)
                {
                    VolumeMaker.Play2DSoundIfCloseToCamera(healthHitSound, transform.position, 20f, 0.1f);
                    playerScript.currentHealth -= damage;
                }
            }
            else if (col.CompareTag("Friendly"))
            {
                Friendly friendlyAI = col.GetComponent<Friendly>();
                if (friendlyAI != null)
                {
                    VolumeMaker.Play2DSoundIfCloseToCamera(healthHitSound, transform.position, 20f  , 0.1f);
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