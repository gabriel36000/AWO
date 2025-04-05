using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Respawn : MonoBehaviour
{
    public GameObject player;
    public GameObject respawnPrefab;
    public GameObject[] respawns;
    public Button respawnButton;

    private ShieldBarScript shield;
    private Player playerScript;

    void Start()
    {
        player = GameObject.Find("Player");

        if (player != null)
        {
            playerScript = player.GetComponent<Player>();
            GameObject shieldBar = GameObject.Find("ShieldBarColor");
            if (shieldBar != null)
                shield = shieldBar.GetComponent<ShieldBarScript>();
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }

    public void OnClick()
    {
        if (playerScript != null)
        {
            player.transform.position = respawnPrefab.transform.position;
            playerScript.currentHealth = playerScript.maxHealth;

            if (shield != null)
                shield.currentShield = shield.maxShield;

            player.SetActive(true);
        }
    }
}