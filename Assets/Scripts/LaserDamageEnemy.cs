﻿using System.Collections;
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
        if (col.CompareTag("Player"))
        {
            int finalDamage = damage;

            if (playerScript.currentShield > 0)
            {
                VolumeMaker.Play2DSoundIfCloseToCamera(shieldHitSound, transform.position, 20f, 0.5f);
                playerScript.currentShield -= damage;
                playerScript.lastShieldRegenTime = Time.time;
            }
            else if (playerScript != null)
            {
                VolumeMaker.Play2DSoundIfCloseToCamera(healthHitSound, transform.position, 20f, 0.1f);
                finalDamage = Mathf.RoundToInt(damage * (1f - (playerScript.currentArmor / 100f)));
                playerScript.currentHealth -= finalDamage;
            }

            ShowDamageEffect(finalDamage);
            Destroy(gameObject);
        }
        else if (col.CompareTag("Friendly"))
        {
            int finalDamage = damage;

            Friendly friendlyAI = col.GetComponent<Friendly>();
            if (friendlyAI != null)
            {
                VolumeMaker.Play2DSoundIfCloseToCamera(healthHitSound, transform.position, 20f, 0.1f);
                friendlyAI.currentHealth -= finalDamage;
            }
            else
            {
                FriendlyTurret turret = col.GetComponent<FriendlyTurret>();
                if (turret != null)
                {
                    VolumeMaker.Play2DSoundIfCloseToCamera(healthHitSound, transform.position, 20f, 0.1f);
                    turret.currentHealth -= finalDamage;
                }
            }

            ShowDamageEffect(finalDamage);
            Destroy(gameObject);
        }
    }
    void ShowDamageEffect(int amount)
    {
        if (damageEffect != null)
        {
            GameObject instance = Instantiate(damageEffect, transform.position, transform.rotation);
            Destroy(instance, 1f);
        }

        if (PopUpPreFab != null)
        {
            GameObject PopUpDamage = Instantiate(PopUpPreFab, transform.position, Quaternion.identity);
            PopUpDamage.transform.GetChild(0).GetComponent<TextMeshPro>().text = amount.ToString();
            Destroy(PopUpDamage, 0.7f);
        }
    }
}