using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealingCenter : MonoBehaviour
{
    public float healRate = 1f; // Heal every second

    // Leave this for Player logic — DO NOT touch
    private Dictionary<MonoBehaviour, float> healTimers = new Dictionary<MonoBehaviour, float>();

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            HandlePlayerHealing(player); // untouched
            return;
        }

        Friendly friendly = collision.GetComponent<Friendly>();
        if (friendly != null)
        {
            HandleFriendlyHealing(friendly); // ✅ this is what we’ll clean up
        }
    }

    private void HandlePlayerHealing(Player player)
    {
        if (!healTimers.ContainsKey(player))
            healTimers[player] = 0;

        if (Time.time > healTimers[player] + healRate)
        {
            int healAmount = Mathf.CeilToInt(player.maxHealth * 0.01f);
            int shieldAmount = Mathf.CeilToInt(player.maxShield * 0.01f);

            player.currentHealth = Mathf.Min(player.currentHealth + healAmount, player.maxHealth);
            player.currentShield = Mathf.Min(player.currentShield + shieldAmount, player.maxShield);

            if (player.healingTextGameObject != null && healAmount > 0 && player.currentHealth < player.maxHealth)
            {
                GameObject healPopUp = Instantiate(player.healingTextGameObject, player.transform.position, Quaternion.identity);
                healPopUp.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + healAmount;
                Destroy(healPopUp, 1f);
            }

            if (player.shieldTextGameObject != null && shieldAmount > 0 && player.currentShield < player.maxShield)
            {
                GameObject shieldPopUp = Instantiate(player.shieldTextGameObject, player.transform.position, Quaternion.identity);
                shieldPopUp.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + shieldAmount;
                Destroy(shieldPopUp, 1f);
            }

            healTimers[player] = Time.time;
        }
    }

    // ✅ Friendly-only healing logic
    private Dictionary<int, float> friendlyHealTimers = new Dictionary<int, float>();

    private void HandleFriendlyHealing(Friendly friendly)
    {
        int id = friendly.GetInstanceID();

        if (!friendlyHealTimers.ContainsKey(id))
            friendlyHealTimers[id] = 0f;

        // Heal once per second
        if (Time.time > friendlyHealTimers[id] + healRate && friendly.currentHealth < friendly.maxHealth)
        {
            int healAmount = Mathf.CeilToInt(friendly.maxHealth * 0.25f); // 1% of max health per second
            int before = friendly.currentHealth;
            friendly.currentHealth = Mathf.Min(friendly.currentHealth + healAmount, friendly.maxHealth);
            int actualHealed = friendly.currentHealth - before;

            if (actualHealed > 0 && friendly.healingTextGameObject != null)
            {
                GameObject healPopUp = Instantiate(friendly.healingTextGameObject, friendly.transform.position, Quaternion.identity);
                healPopUp.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + actualHealed;
                Destroy(healPopUp, 1f);
            }

            friendlyHealTimers[id] = Time.time;
        }
    }
}