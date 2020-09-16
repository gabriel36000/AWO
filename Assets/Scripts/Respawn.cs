using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Respawn : MonoBehaviour
{
    public GameObject player;
    public GameObject respawnPrefab;
    public GameObject[] respawns;
    public GameObject healthBar;
    public GameObject shieldBar;
    public Button respawnButton;
    public ShieldBarScript shield;
    public HealthBar health;
    public void Start() {
        player = GameObject.Find("Player");
        healthBar = GameObject.Find("HealthBarColor");
        shieldBar = GameObject.Find("ShieldBarColor");
        shield = shieldBar.GetComponent<ShieldBarScript>();
        health = healthBar.GetComponent<HealthBar>();

    }

    public void OnClick() {
        player.transform.position = respawnPrefab.transform.position;
        health.currentHealth = health.maxHealth;
        shield.currentShield = shield.maxShield;
        player.SetActive(true);
    }
        
}
