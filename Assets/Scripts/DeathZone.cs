using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DeathZone : MonoBehaviour
{
    float timeLeft = 10.0f;
    public TextMeshProUGUI Message;
    public GameObject messageGameObject;
    public Player player;
    public GameObject Shield;
    public GameObject player1;
    bool TimerStarted = false;
    HealthBar healthbar;
    ShieldBarScript shieldBar;

    public void Start() {
        player1 = GameObject.Find("Player");
        player = player1.GetComponent<Player>();
        Shield = GameObject.Find("ShieldBarColor");
        shieldBar = Shield.GetComponent<ShieldBarScript>();
    }

    public void Update() {
        if (TimerStarted) {
            timeLeft -= Time.deltaTime;
            Message.text = "You have " + timeLeft.ToString("0") + " to get back to the map!!";

            if (timeLeft <= 0) {
                player.currentHealth = 0;
                shieldBar.currentShield = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            if (!TimerStarted) TimerStarted = true;
            {
                messageGameObject.SetActive(true);
            }                              
    
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            messageGameObject.SetActive(false);
            TimerStarted = false;
            Message.text = "";
            timeLeft = 10.0f;
        }
               
    }
}
