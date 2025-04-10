using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaserDamage : MonoBehaviour
{
    public int damage;

    Player player;
    public GameObject player1;
    public GameObject PopUpPreFab;
    Enemy enemy;
    public GameObject damageEffect;
   






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
        // Try to get player reference if it's null
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

        // Now proceed with normal collision logic
        if (col.CompareTag("Enemy") || col.CompareTag("Boss"))
        {
            col.GetComponent<Enemy>().currentHealth -= damage;

            GameObject instance = Instantiate(damageEffect, transform.position, transform.rotation);
            Destroy(instance, 1f);

            Destroy(gameObject);

            GameObject PopUpDamage = Instantiate(PopUpPreFab, transform.position, Quaternion.identity);
            PopUpDamage.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
            Destroy(PopUpDamage, 0.7f);
        }
    }
}
