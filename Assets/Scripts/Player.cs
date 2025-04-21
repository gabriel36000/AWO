using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SimpleJSON;

public class Player : MonoBehaviour
{
    // Bonus Stats:
    public int bonusHealth = 0;
    public int bonusShield = 0;
    public int bonusDamage = 0;
    public int bonusSpeed = 0;
    public int bonusCritChance = 0;
    public int bonusArmor = 0;
    // Exp

    private string Name;
    private int Exp;
    private int totalExp;

    // Health
    public int maxHealth = 100;
    public int currentHealth;
    public TextMeshProUGUI healthText;
    private Image healthBarColor;
    public GameObject smoke;
    public float healthRegenRate = 1f;
    public float lastHealthRegenTime;
    public GameObject lowHealthEffect;
    public Gradient healthGradient;

    // Shield
    public int maxShield = 1000;
    public int currentShield;
    public TextMeshProUGUI shieldText;
    private Image shieldBarColor;
    public GameObject bubbleShield;
    public GameObject lighting;
    public GameObject lightingShieldDown;
    public float shieldRegenRate = 20f;
    public float lastShieldRegenTime;

    // Damage
    public int minDamage;
    public int maxDamage;
    public int damage;
    public int criticalChance = 0;

    // Other
    public GameObject healthBar1;
    public GameObject shieldBar1;
    public GameObject player;
    public GameObject button;
    public CharacterStatsManager stats;
    public PlayerMovement playerMovement;
    public GameObject healingTextGameObject;
    public GameObject shieldTextGameObject;
    public GameObject explosion;
    public VolumeMaker volumeMaker;
    public AudioClip explosionSound;
    public float fireRateDelay = 0.5f;
    public float currentArmor = 0;

    LevelSystem levelSystem;

    // Skills
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
    public int criticalSkill = 0;
    public TextMeshProUGUI criticalText;
    public int rateOfFireSkill = 0;
    public TextMeshProUGUI rateOfFireText;
    public int armorSkill = 0;
    public TextMeshProUGUI armorText;

    // Saving variables
    private int Hp;
    private int Shield;

    void Start()
    {
        healthBar1 = GameObject.Find("HealthBarColor");
        shieldBar1 = GameObject.Find("ShieldBarColor");
        player = GameObject.Find("Player");

        levelSystem = player.GetComponent<LevelSystem>();
        stats = player.GetComponent<CharacterStatsManager>();
        playerMovement = player.GetComponent<PlayerMovement>();

        healthBarColor = healthBar1.GetComponent<Image>();
        shieldBarColor = shieldBar1.GetComponent<Image>();

        currentHealth = maxHealth;
        currentShield = maxShield;

        lastHealthRegenTime = Time.time;
        lastShieldRegenTime = Time.time;

        button.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) Save();
        if (Input.GetKeyDown(KeyCode.L)) Load();

        damage = Random.Range(minDamage, maxDamage);

        RegenerateHealth();
        RegenerateShield();

        UpdateHealthBar();
        UpdateShieldBar();

        SmokeEffect();
        ShieldEffects();
        LowHealthWarning();
        CheckDeath();
    }

    // ========== Health Logic ==========
    public void UpdateHealthBar()
    {
        if (healthBarColor != null)
        {
            healthBarColor.fillAmount = (float)currentHealth / maxHealth;
            if (healthText != null)
            {
                healthText.text = "HP: " + currentHealth;
            }
            healthBarColor.color = healthGradient.Evaluate(healthBarColor.fillAmount);
        }
    }

    void RegenerateHealth()
    {
        if (Time.time > lastHealthRegenTime + healthRegenRate && currentHealth < maxHealth)
        {
            int healAmount = Mathf.CeilToInt(maxHealth * 0.01f);
            currentHealth += healAmount;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            if (healingTextGameObject != null)
            {
                GameObject healPopUp = Instantiate(healingTextGameObject, transform.position, Quaternion.identity);
                healPopUp.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + healAmount.ToString();
                Destroy(healPopUp, 1f);
            }

            lastHealthRegenTime = Time.time;
        }
    }

    void SmokeEffect()
    {
        if (smoke != null)
            smoke.SetActive(currentHealth <= maxHealth / 4);
    }

    void LowHealthWarning()
    {
        if (lowHealthEffect != null)
            lowHealthEffect.SetActive(currentHealth <= maxHealth / 4);
    }

    // ========== Shield Logic ==========
    public void UpdateShieldBar()
    {
        if (shieldBarColor != null)
        {
            shieldBarColor.fillAmount = (float)currentShield / maxShield;
            if (shieldText != null)
            {
                shieldText.text = "Shield: " + currentShield;
            }
        }
    }

    public void RegenerateShield()
    {
        if (currentShield < 0)
            currentShield = 0;

        // Safer health check (>= instead of ==)
        if (currentHealth >= maxHealth && currentShield < maxShield)
        {
            if (Time.time > lastShieldRegenTime + shieldRegenRate)
            {
                int shieldRegenAmount = Mathf.CeilToInt(maxShield * 0.10f);
                currentShield += shieldRegenAmount;

                if (healingTextGameObject != null)
                {
                    GameObject shieldPopUp = Instantiate(healingTextGameObject, transform.position, Quaternion.identity);
                    shieldPopUp.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + shieldRegenAmount.ToString();
                    shieldPopUp.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.cyan;
                    Destroy(shieldPopUp, 1f);
                }

                if (currentShield > maxShield)
                    currentShield = maxShield;

                lastShieldRegenTime = Time.time;
            }
        }
    }

    void ShieldEffects()
    {
        if (currentShield <= 0)
        {
            if (bubbleShield != null) bubbleShield.SetActive(false);
            if (lighting != null) lighting.SetActive(false);
            if (lightingShieldDown != null) lightingShieldDown.SetActive(true);
        }
        else
        {
            if (bubbleShield != null) bubbleShield.SetActive(true);
            if (lighting != null) lighting.SetActive(true);
            if (lightingShieldDown != null) lightingShieldDown.SetActive(false);
        }
    }

    // ========== Death ==========    
    void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            VolumeMaker.Play2DSoundIfCloseToCamera(explosionSound, transform.position, 20f, 0.3f);
            Instantiate(explosion, transform.position, transform.rotation);
            gameObject.SetActive(false);
            if (button != null) button.SetActive(true);
            if (levelSystem != null) levelSystem.penalty();
        }
        else
        {
            if (button != null) button.SetActive(false);
        }
    }

    // ========== Saving & Loading ==========
    public void Save()
    {
        Name = "PlayerOne";
        Exp = levelSystem.currentXP;
        totalExp = levelSystem.totalXP;
        Hp = currentHealth;
        Shield = currentShield;

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

    public void Load()
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
        currentShield = Shield;
        levelSystem.currentXP = Exp;
        levelSystem.totalXP = totalExp;
    }

    // ========== Skill Upgrades ==========
    public void IncreaseHP()
    {
        if (stats.currentSkillPoints > 0)
        {
            stats.currentSkillPoints--;
            hpSkill++;
            float newHealth = maxHealth * (1 + hpSkill * 0.05f);
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
            float newShield = maxShield * (1 + shieldSkill * 0.05f);
            SetMaxShield(Mathf.RoundToInt(newShield - maxShield));
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
            float newMinDamage = minDamage * (1 + damageSkill * 0.05f);
            SetMinDamage(Mathf.RoundToInt(newMinDamage - minDamage));
            float newMaxDamage = maxDamage * (1 + damageSkill * 0.05f);
            SetMaxDamage(Mathf.RoundToInt(newMaxDamage - maxDamage));
            tmpDamage.text = damageSkill.ToString();
            tmp.text = stats.currentSkillPoints.ToString();
        }
    }
    public void IncreaseRateOfFire()
    {
        if (stats.currentSkillPoints > 0)
        {
            stats.currentSkillPoints--;
            rateOfFireSkill++;
            fireRateDelay *= 0.97f; // Reduce delay by 3%
            fireRateDelay = Mathf.Clamp(fireRateDelay, 0.1f, 1f);
            rateOfFireText.text = rateOfFireSkill.ToString();
            tmp.text = stats.currentSkillPoints.ToString();
        }
    }

    public void IncreaseSpeed()
    {
        if (stats.currentSkillPoints > 0)
        {
            stats.currentSkillPoints--;
            speedSkill++;
            SetSpeed(1);
            SetBoostSpeed(1);
            speedText.text = speedSkill.ToString();
            tmp.text = stats.currentSkillPoints.ToString();
        }
    }

    public void IncreaseCriticalChance()
    {
        if (stats.currentSkillPoints > 0 && criticalChance < 100)
        {
            stats.currentSkillPoints--;
            criticalSkill++;
            SetCriticalChance(1);
            criticalText.text = criticalSkill.ToString();
            tmp.text = stats.currentSkillPoints.ToString();
        }
        else
        {
            Debug.Log("Max critical chance reached!");
        }
    }
    public void IncreaseArmor()
    {
        if (stats.currentSkillPoints > 0)
        {
            stats.currentSkillPoints--;
            armorSkill++;
            float newArmor = currentArmor + 0.25f; 
            float armorIncrease = newArmor - currentArmor;
            SetArmor(Mathf.RoundToInt(armorIncrease));
            currentArmor = newArmor;
            armorText.text = armorSkill.ToString();
            tmp.text = stats.currentSkillPoints.ToString();
        }
    }
    public void GainXP(int amount)
    {
        LevelSystem levelSystem = GetComponent<LevelSystem>();
        if (levelSystem != null)
        {
            levelSystem.AddXP(amount);
        }
    }

    // ========== Stat Setters ==========
    public void SetMaxHealth(int amount) => maxHealth += amount;
    public void SetMaxShield(int amount) => maxShield += amount;
    public void SetMinDamage(int amount) => minDamage += amount;
    public void SetMaxDamage(int amount) => maxDamage += amount;
    public void SetSpeed(int amount) => playerMovement.maxSpeed += amount;
    public void SetBoostSpeed(int amount) => playerMovement.MaxBoostspeed += amount;
    public void SetCriticalChance(int amount) => criticalChance += amount;
    public void SetRateOfFire(int amount) => fireRateDelay += amount;
    
    public void SetArmor(int amount) => currentArmor += amount;
}