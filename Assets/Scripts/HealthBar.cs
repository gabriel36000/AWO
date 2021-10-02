using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour {
    Image HealthBarColor;
    public int maxHealth = 100;
    public int currentHealth;
    //public Death death;
    public TextMeshProUGUI health;
    public GameObject smoke;
    public float regenrate = 1f;
    public float lastTime;
    public GameObject lowHealthEffect;
    public Gradient gradient;

    // Use this for initialization
    void Start() {
        HealthBarColor = GetComponent<Image>();
        currentHealth = maxHealth;
        lastTime = Time.time;


    }

    public void SetMaxHealth(int health) {
        maxHealth += health;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate() {
        HealthBarColor.fillAmount = (float)currentHealth / maxHealth;
        health.text = "HP: " + (currentHealth);
        HealthBarColor.color = gradient.Evaluate(HealthBarColor.fillAmount);
        Smoke();
        regenerating();
        LowHealth();

    }
    public void Smoke() {
        if (currentHealth <= maxHealth / 4) {
            smoke.SetActive(true);
        }
        else {
            smoke.SetActive(false);
        }
    }

    public void regenerating() {
        if ((Time.time > lastTime + regenrate) && (currentHealth < maxHealth)) {
            currentHealth += 1;
            lastTime = Time.time;


        }
    }
    public void LowHealth() {
        if(currentHealth <= maxHealth / 4) {
            lowHealthEffect.SetActive(true);
        }
        else {
            lowHealthEffect.SetActive(false);
        }
    }
}