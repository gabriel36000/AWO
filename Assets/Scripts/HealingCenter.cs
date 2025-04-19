using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealingCenter : MonoBehaviour
{
    public float healRate = 0.25f; // Heal every 0.25 seconds
    private float lastHealTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && Time.time > lastHealTime + healRate)
        {
            if (player.currentHealth < player.maxHealth || player.currentShield < player.maxShield)
            {
                int healAmount = Mathf.CeilToInt(player.maxHealth * 0.01f);
                int shieldAmount = Mathf.CeilToInt(player.maxShield * 0.01f);
                player.currentHealth += healAmount;
                player.currentShield += shieldAmount;

                if (player.currentHealth > player.maxHealth)
                {
                    player.currentHealth = player.maxHealth;
                }
                
             
                if (player.currentShield > player.maxShield)
                {
                    player.currentShield = player.maxShield;
                }
                 

                if (player.healingTextGameObject != null)
                {
                    if (player.currentHealth < player.maxHealth)
                    {
                        GameObject healPopUp = Instantiate(player.healingTextGameObject, player.transform.position, Quaternion.identity);
                        healPopUp.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + healAmount;
                        Destroy(healPopUp, 1f);
                    }
                }
                if (player.shieldTextGameObject != null) 
                {
                    if(player.currentShield < player.maxShield)
                    {
                        GameObject shieldPopUp = Instantiate(player.shieldTextGameObject, player.transform.position, Quaternion.identity);
                        shieldPopUp.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + shieldAmount;
                        Destroy(shieldPopUp, 1f);
                    }
                }
                lastHealTime = Time.time;
            }
        }
    }
}
