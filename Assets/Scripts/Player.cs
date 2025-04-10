using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SimpleJSON;

public class Player : MonoBehaviour {
    public string Name;

    public int Exp;

    private int Hp;
    private int Shield;
    private int speed;
    public int minDamage;
    public int maxDamage;
    public int damage;
    public GameObject healthBar1;
    public GameObject shieldBar1;
    public GameObject player;
    public CharacterStatsManager stats;
    public GameObject PopUpHealth;
    public float percentPerPoint = 0.05f;
    public GameObject button;

    LevelSystem levelSystem;
    ShieldBarScript shieldBar;

    LaserDamage laser;

    // === HealthBar Integration ===
    public int maxHealth = 100;
    public int currentHealth;
    public TextMeshProUGUI healthText;
    public GameObject smoke;
    public float regenrate = 1f;
    public float lastTime;
    public GameObject lowHealthEffect;
    public Gradient gradient;
    private Image healthBarColor;

    // === Skills ===
    [Header("Skills")]
    public TextMeshProUGUI tmpHp;
    public int hpSkill = 0;
    public TextMeshProUGUI tmpShield;
    public int shieldSkill = 0;
    public TextMeshProUGUI tmpDamage;
    public int damageSkill = 0;
    public TextMeshProUGUI tmp;
    public int speedSkill = 0;
    public TextMeshProUGUI speedText;

    public void IncreaseHP()
    {
        if (stats.currentSkillPoints > 0)
        {
            stats.currentSkillPoints--;
            hpSkill++;
            float newHealth = maxHealth * (1 + hpSkill * percentPerPoint);
            SetMaxHealth(Mathf.RoundToInt(newHealth - maxHealth));
            tmpHp.text = hpSkill.ToString();
            tmp.text = stats.currentSkillPoints.ToString();
        }
    }

    public void IncreaseShield()
    {
        if (stats.currentSkillPoints > 0)
        {
            stats.currentSkillPoints--;
            shieldSkill++;
            tmpShield.text = shieldSkill.ToString();
            tmp.text = stats.currentSkillPoints.ToString();
        }
    }

    public void IncreaseDamage()
    {
        if (stats.currentSkillPoints > 0)
        {
            stats.currentSkillPoints--;
            damageSkill++;
            float newMinDamage = minDamage * (1 + damageSkill * percentPerPoint);
            SetMinDamage(Mathf.RoundToInt(newMinDamage - minDamage));
            float newMaxDamage = maxDamage * (1 + damageSkill * percentPerPoint);
            SetMaxDamage(Mathf.RoundToInt(newMaxDamage - maxDamage));
            tmpDamage.text = damageSkill.ToString();
            tmp.text = stats.currentSkillPoints.ToString();
        }
    }

    public void Start()
    {
        healthBar1 = GameObject.Find("HealthBarColor");
        shieldBar1 = GameObject.Find("ShieldBarColor");
        player = GameObject.Find("Player");

        levelSystem = player.GetComponent<LevelSystem>();
        shieldBar = shieldBar1.GetComponent<ShieldBarScript>();
        stats = player.GetComponent<CharacterStatsManager>();

        healthBarColor = healthBar1.GetComponent<Image>();
        currentHealth = maxHealth;
        lastTime = Time.time;
        button.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) Save();
        if (Input.GetKeyDown(KeyCode.L)) Load();
        damage = Random.Range(minDamage, maxDamage);

        UpdateHealthBar();
        Regenerate();
        Smoke();
        LowHealth();
        CheckDeath(); 
    }

    // === HealthBar Logic ===
    void UpdateHealthBar()
    {
        if (healthBarColor != null)
        {
            healthBarColor.fillAmount = (float)currentHealth / maxHealth;
            if (healthText != null)
            {
                healthText.text = "HP: " + currentHealth;
            }
            healthBarColor.color = gradient.Evaluate(healthBarColor.fillAmount);
        }
    }

    void Smoke()
    {
        if (smoke != null)
        {
            smoke.SetActive(currentHealth <= maxHealth / 4);
        }
    }

    void Regenerate()
    {
        if ((Time.time > lastTime + regenrate) && (currentHealth < maxHealth))
        {
            currentHealth += 1;
            lastTime = Time.time;
        }
    }

    void LowHealth()
    {
        if (lowHealthEffect != null)
        {
            lowHealthEffect.SetActive(currentHealth <= maxHealth / 4);
        }
    }

    // === Saving & Loading ===
    public void Save()
    {
        Exp = levelSystem.xp;
        Hp = currentHealth;
        Shield = shieldBar.currentShield;

        JSONObject playerJson = new JSONObject();
        playerJson.Add("Name", Name);
        playerJson.Add("Exp", Exp);
        playerJson.Add("HP", Hp);
        playerJson.Add("Shield", Shield);

        JSONArray position = new JSONArray();
        position.Add(transform.position.x);
        position.Add(transform.position.y);
        position.Add(transform.position.z);
        playerJson.Add("Position", position);

        string path = Application.persistentDataPath + "/PlayerSave.json";
        File.WriteAllText(path, playerJson.ToString());
    }

    void Load()
    {
        string path = Application.persistentDataPath + "/PlayerSave.json";
        if (!File.Exists(path)) return;

        string jsonString = File.ReadAllText(path);
        JSONObject playerJson = (JSONObject)JSON.Parse(jsonString);

        Name = playerJson["Name"];
        Exp = playerJson["Exp"];
        Hp = playerJson["HP"];
        Shield = playerJson["Shield"];

        transform.position = new Vector3(
            playerJson["Position"].AsArray[0],
            playerJson["Position"].AsArray[1],
            playerJson["Position"].AsArray[2]
        );

        currentHealth = Hp;
        shieldBar.currentShield = Shield;
        levelSystem.xp = Exp;
    }
    public void SetMaxHealth(int health)
    {
        maxHealth += health;
        currentHealth = maxHealth;
    }

    public void SetMinDamage(int SetminDamage)
    {
        minDamage += SetminDamage;
        
    }

    public void SetMaxDamage(int SetmaxDamage1)
    {
        maxDamage += SetmaxDamage1;
        
    }

    void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Death");
            gameObject.SetActive(false); // Disable player GameObject
            if (button != null) button.SetActive(true);
            if (levelSystem != null) levelSystem.penalty();
        }
        else
        {
            if (button != null) button.SetActive(false);
        }
    }
}