using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterStatsManager : MonoBehaviour
{
    public int currentSkillPoints;
    public TextMeshProUGUI maxHealth;
    public TextMeshProUGUI maxShield;
    public TextMeshProUGUI maxMovementText;
    public TextMeshProUGUI skillPointGUI;
    public GameObject movementStats;
    public GameObject health;
    public GameObject Shield;
    PlayerMovement movement;
    HealthBar healthbar;
    ShieldBarScript shieldBar;
    void Start()
    {
        movementStats = GameObject.Find("Player");
        movement = movementStats.GetComponent<PlayerMovement>();
        health = GameObject.Find("HealthBarColor");
        healthbar = health.GetComponent<HealthBar>();
        Shield = GameObject.Find("ShieldBarColor");
        shieldBar = Shield.GetComponent<ShieldBarScript>();
    }

    
    void Update()
    {
        maxMovementText.text = movement.maxSpeed.ToString("0");
        maxHealth.text = healthbar.maxHealth.ToString("0");
        maxShield.text = shieldBar.maxShield.ToString("0");
        skillPointGUI.text = currentSkillPoints.ToString("0");


    }
}
