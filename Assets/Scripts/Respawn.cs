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

    public void OnClick() {
        player.transform.position = respawnPrefab.transform.position;
        HealthBar.currentHealth = HealthBar.maxHealth;
        ShieldBarScript shield = new ShieldBarScript();
        ShieldBarScript.currentShield = shield.maxShield;
        LevelSystem peanlty0 = new LevelSystem();
        peanlty0.penalty();
        player.SetActive(true);
    }
        
}
