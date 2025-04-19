using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FriendlyLaserDamage : MonoBehaviour
{
    public int damage;
    private Friendly friendly;
    private FriendlyTurret friendlyTurret; // lowercase variable name (good practice)
    public GameObject PopUpPreFab;
    public GameObject PopUpPreFabCritical;
    public GameObject damageEffect;
    public float criticalMultiplier = 2f;
    public VolumeMaker volumeMaker;
    public AudioClip healthHitSound;

    // Setup for Friendly
    public void Setup(Friendly owner)
    {
        friendly = owner;
        damage = friendly.damage;
    }

    // Setup for FriendlyTurret
    public void Setup(FriendlyTurret turret)
    {
        friendlyTurret = turret;
        damage = turret.damage;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (friendly == null && friendlyTurret == null)
        {
            Debug.LogWarning("Friendly or Turret reference missing on laser!");
            Destroy(gameObject);
            return;
        }

        if (col.CompareTag("Enemy") || col.CompareTag("Boss"))
        {
            int finalDamage = damage;

            // Use whichever source is assigned
            int critChance = 0;
            if (friendly != null)
                critChance = friendly.criticalChance;
            else if (friendlyTurret != null)
                critChance = friendlyTurret.criticalChance;

            // Critical hit check
            bool isCrit = Random.Range(0, 100) < critChance;
            if (isCrit)
            {
                finalDamage = Mathf.RoundToInt(damage * criticalMultiplier);

                GameObject critPopUp = Instantiate(PopUpPreFabCritical, transform.position, Quaternion.identity);
                if (critPopUp.transform.childCount > 0)
                {
                    var textMesh = critPopUp.transform.GetChild(0).GetComponent<TextMeshPro>();
                    if (textMesh != null)
                        textMesh.text = "CRIT! " + finalDamage;
                }
                Destroy(critPopUp, 0.7f);
            }
            else
            {
                GameObject popUpDamage = Instantiate(PopUpPreFab, transform.position, Quaternion.identity);
                if (popUpDamage.transform.childCount > 0)
                {
                    var textMesh = popUpDamage.transform.GetChild(0).GetComponent<TextMeshPro>();
                    if (textMesh != null)
                        textMesh.text = finalDamage.ToString();
                }
                Destroy(popUpDamage, 0.7f);
            }

            VolumeMaker.Play2DSoundIfCloseToCamera(healthHitSound, transform.position, 20f, 0.1f);

            // Damage enemy
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.currentHealth -= finalDamage;
            }

            // Damage effect
            if (damageEffect != null)
            {
                GameObject instance = Instantiate(damageEffect, transform.position, transform.rotation);
                Destroy(instance, 1f);
            }

            Destroy(gameObject);
        }
    }
}