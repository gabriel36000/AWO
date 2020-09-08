using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    Image HealthBarColor;
    public static int maxHealth = 100;
    public static int currentHealth;
    public Death death;
    public TextMeshProUGUI health;
    public GameObject smoke;

    // Use this for initialization
    void Start()
    {
        HealthBarColor = GetComponent<Image>();
        currentHealth = maxHealth;
       

    }

   public void SetMaxHealth(int health) {
        maxHealth += health;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HealthBarColor.fillAmount = (float)currentHealth / maxHealth;
        health.text = "HP: " + (currentHealth);
        Smoke();

    }
    public void Smoke() {
        if (currentHealth <= 50) {
            smoke.SetActive(true);
        }
        else {
            smoke.SetActive(false);
        }
    }
    

    
}