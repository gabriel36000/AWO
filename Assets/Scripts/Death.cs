using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    
    public GameObject button;
    public GameObject player;
    public GameObject healthCurrent;
    LevelSystem levelSystem;
    HealthBar health;
    void Start()
    {
        
        player = GameObject.Find("Player");
        
        healthCurrent = GameObject.Find("HealthBarColor");
        levelSystem = player.GetComponent<LevelSystem>();
        health = healthCurrent.GetComponent<HealthBar>();
        
    }

    // Update is called once per frame
    void Update() {
        if (health.currentHealth <= 0) {
            Debug.Log("Death");
            player.SetActive(false);
            button.SetActive(true);
            levelSystem.penalty();
            //Destroy(gameObject); Instead of destorying the GameObject, disable it in order for the player to respawn properly.
           
            
        }
        else {
            button.SetActive(false);
            
        }

    }
}

