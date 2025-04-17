using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public int level = 1;
    public int currentXP = 0;
    public int totalXP = 0;
    public int maxLevel = 100;

    public TextMeshPro levelText;
    public Image xpBar;

    Player player;
    CharacterStatsManager statsManager;

    void Start()
    {
        player = GetComponent<Player>();
        statsManager = GetComponent<CharacterStatsManager>();
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddXP(25);
        }
        UpdateUI();
    }

    public void AddXP(int amount)
    {
        totalXP += amount;
        currentXP += amount;

        while (currentXP >= GetXPForLevel(level) && level < maxLevel)
        {
            currentXP -= GetXPForLevel(level);
            level++;
            LevelUp();
        }

        UpdateUI();
    }

    void LevelUp()
    {
        statsManager.currentSkillPoints++;
        player.SetMaxHealth(GetHealthForLevel(level));
        player.SetMaxShield(GetShieldForLevel(level));
        Debug.Log("Level Up! Now level: " + level);
    }

    void UpdateUI()
    {
        levelText.text = "Level: " + level;
        int xpForNext = GetXPForLevel(level);
        if (xpForNext > 0)
        {
            xpBar.fillAmount = (float)currentXP / xpForNext;
        }
        else
        {
            xpBar.fillAmount = 1f; // Just fill bar if you're at max level
        }
    }

    int GetXPForLevel(int lvl)
    {
        float baseXP = 100f;
        float exponent = 1.2f;
        return Mathf.RoundToInt(baseXP * Mathf.Pow(lvl, exponent));
    }

    int GetHealthForLevel(int lvl)
    {
        return 100 + lvl * 10;
    }

    int GetShieldForLevel(int lvl)
    {
        return 200 + lvl * 20;
    }

    public void penalty()
    {
        if (player.currentHealth <= 0)
        {
            currentXP /= 2;
            currentXP = Mathf.Clamp(currentXP / 2, 0, GetXPForLevel(level));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("XP"))
        {
            AddXP(40);
            Destroy(collision.gameObject);
        }
    }
}