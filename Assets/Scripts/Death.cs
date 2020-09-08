using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public GameObject button;
    public GameObject player;
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {
        if (HealthBar.currentHealth <= 0) {
            Debug.Log("Death");
            LevelSystem peanlty0 = new LevelSystem();
            peanlty0.penalty();
            //Destroy(gameObject); Instead of destorying the GameObject, disable it in order for the player to respawn properly.
            player.SetActive(false);
            button.SetActive(true);
            
        }
        else {
            button.SetActive(false);
            
        }

    }
}

