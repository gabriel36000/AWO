using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathZone : MonoBehaviour
{
    float timeLeft = 10.0f;
    public TextMeshProUGUI Message;
    public GameObject messageGameObject;
    public GameObject health;
    public GameObject Shield;
    bool TimerStarted = false;
    HealthBar healthbar;
    ShieldBarScript shieldBar;

    public void Start() {
        health = GameObject.Find("HealthBarColor");
        healthbar = health.GetComponent<HealthBar>();
        Shield = GameObject.Find("ShieldBarColor");
        shieldBar = Shield.GetComponent<ShieldBarScript>();
    }

    public void Update() {
        if (TimerStarted) {
            timeLeft -= Time.deltaTime;
            Message.text = "You have " + timeLeft.ToString("0") + " to get back to the map!!";

            if (timeLeft <= 0) {
                healthbar.currentHealth = 0;
                shieldBar.currentShield = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            if (!TimerStarted) TimerStarted = true;
            {
                
            }                              
    
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            TimerStarted = false;
            Message.text = "";
            timeLeft = 10.0f;
        }
               
    }
}
