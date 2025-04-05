using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour {
    public int xp = 0;
    public int level = 0;
    public int[] xpNeeded = { 100, 200, 300, 400 };
    int[] levelHealth = { 10, 30, 50, 100 };
    int[] levelShield = { 100, 200, 300, 400 };
    public TextMeshPro levelText;
    public ShieldBarScript shieldBar;
    public Image xpBackground;
    public GameObject playerHealth;
    BoxCollider2D bx;
    Player player;
    CharacterStatsManager statsManager;


    void Start() {
        player = GetComponent<Player>();
        playerHealth = GameObject.Find("Player");
        statsManager =  GetComponent<CharacterStatsManager>();
        bx = GetComponent<BoxCollider2D>();
        



    }

    void CheckLevel() {
        if (level > xpNeeded.Length - 1) {

            return;
        }
        if (xp >= xpNeeded[level]) {
            Levelup();
            level++;
            print("Level up");
        }
    }
    void Update() {

        
        if (level == 0) {
          xpBackground.fillAmount = (float)(xp) / (xpNeeded[level]);

        }
        else {
            
            xpBackground.fillAmount = (float)(xp - xpNeeded[level - 1]) / (xpNeeded[level] - xpNeeded[level - 1]);
        }
        
        CheckLevel();
        if (Input.GetKeyDown(KeyCode.K)) {
            xp += 10;
            print(xp);
        }
        

    }
    public void Levelup() {
        levelText.text = "Level: " + (level + 2);
        player.SetMaxHealth(levelHealth[level]);
        shieldBar.SetMaxShield(levelShield[level]);
        statsManager.currentSkillPoints += 1;
        
        
        

    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "XP") {
            xp += 40;
            print(xp);
            Destroy(collision.gameObject);
        }
    }
    public void penalty() {
        if (player.currentHealth <= 0) {
            xp /= 2;
            
            
            
        }
    }
}
