using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterStatsManager : MonoBehaviour
{
    public int currentSkillPoints;

    public TextMeshProUGUI minDmg;
    public TextMeshProUGUI maxDmg;
    public TextMeshProUGUI maxHealth;
    public TextMeshProUGUI maxShield;
    public TextMeshProUGUI maxMovementText;
    public TextMeshProUGUI skillPointGUI;
    public TextMeshProUGUI criticalChanceText;

    GameObject player;
    Player playerScript;
    PlayerMovement movement;
    ShieldBarScript shieldBar;

    void Start()
    {
        player = GameObject.Find("Player");

        if (player != null)
        {
            playerScript = player.GetComponent<Player>();
            movement = player.GetComponent<PlayerMovement>();
            shieldBar = GameObject.Find("ShieldBarColor").GetComponent<ShieldBarScript>();
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }

    void Update()
    {
        if (playerScript != null)
        {
            maxMovementText.text = movement.maxSpeed.ToString("0");
            maxHealth.text = playerScript.maxHealth.ToString("0");
            maxShield.text = shieldBar.maxShield.ToString("0");
            skillPointGUI.text = currentSkillPoints.ToString("0");
            minDmg.text = playerScript.minDamage.ToString("0");
            maxDmg.text = playerScript.maxDamage.ToString("0");
            criticalChanceText.text = playerScript.criticalChance.ToString("0") + "%";

        }
    }
}