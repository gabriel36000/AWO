using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public string Name;
    public int Level;
    public float Exp;
    public int Hp;
    public int maxHP;
    public int maxShield;
    public int Shield;

    LevelSystem levelSystem;
    ShieldBarScript shieldBar;
    HealthBar healthBar;

    public void Start() {
        levelSystem = GetComponent<LevelSystem>();
        shieldBar = GetComponent<ShieldBarScript>();
        healthBar = GetComponent<HealthBar>();

    }
    public void Update() {
        Level = levelSystem.level;
        Level++;
        Exp = levelSystem.xp;

        // maxHP = healthBar.maxHealth;
        // maxShield = shieldBar.maxShield;

    }
}
