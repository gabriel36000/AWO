using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaserDamage : MonoBehaviour
{
    public int damage;
    private Player player;
    public GameObject player1;
    public GameObject PopUpPreFab;
    public GameObject damageEffect;

    public float criticalMultiplier = 2f; // How much to multiply damage on crit (2x by default)

    public void Start()
    {
        player1 = GameObject.Find("Player");

        if (player1 != null)
        {
            player = player1.GetComponent<Player>();
            if (player != null)
            {
                damage = player.damage;
            }
            else
            {
                Debug.LogWarning("Player script missing on Player GameObject.");
                damage = 10; // fallback damage value
            }
        }
        else
        {
            Debug.LogWarning("Player GameObject not found.");
            damage = 10; // fallback damage value
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                player = playerObj.GetComponent<Player>();
                if (player != null)
                {
                    damage = player.damage;
                }
            }
        }

        if (col.CompareTag("Enemy") || col.CompareTag("Boss"))
        {
            int finalDamage = damage;

            // === Critical Hit Calculation ===
            int randomRoll = Random.Range(0, 100); // Random number between 0 and 99
            if (randomRoll < player.criticalChance)
            {
                finalDamage = Mathf.RoundToInt(damage * criticalMultiplier);

                // Optional: spawn special popup for critical
                GameObject critPopUp = Instantiate(PopUpPreFab, transform.position, Quaternion.identity);
                critPopUp.transform.GetChild(0).GetComponent<TextMeshPro>().text = "CRIT! " + finalDamage.ToString();
                Destroy(critPopUp, 0.7f);
            }
            else
            {
                // Normal hit popup
                GameObject popUpDamage = Instantiate(PopUpPreFab, transform.position, Quaternion.identity);
                popUpDamage.transform.GetChild(0).GetComponent<TextMeshPro>().text = finalDamage.ToString();
                Destroy(popUpDamage, 0.7f);
            }

            // Apply damage
            col.GetComponent<Enemy>().currentHealth -= finalDamage;

            // Damage effect
            if (damageEffect != null)
            {
                GameObject instance = Instantiate(damageEffect, transform.position, transform.rotation);
                Destroy(instance, 1f);
            }

            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}