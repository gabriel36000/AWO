using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour {
    Image HealthBarColor;
    public static int maxHealth = 100;
    public static int currentHealth;
    public Death death;
    public TextMeshProUGUI health;
    public GameObject smoke;
    private float regenrate = 1f;
    public float lastTime;

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
        Smoke();
        regenerating();

    }
    public void Smoke() {
        if (currentHealth <= 50) {
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
}