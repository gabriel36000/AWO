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

    private Player playerScript;

    void Start()
    {
        player = GameObject.Find("Player");

        if (player != null)
        {
            playerScript = player.GetComponent<Player>();
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
            playerScript.currentShield = playerScript.maxShield; // ✅ Restore shield directly

            player.SetActive(true);

            // Optional: If you want, update UI immediately too:
            playerScript.UpdateHealthBar();
            playerScript.UpdateShieldBar();
        }
    }
}