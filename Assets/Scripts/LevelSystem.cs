using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour {
    public int xp = 0;
    int level = 0;
    int[] xpNeeded = { 100, 200, 300, 400 };
    int[] levelHealth = { 10, 30, 50, 100 };
    int[] levelShield = { 100, 200, 300, 400 };
    public TextMeshPro levelText;
    public HealthBar healthBar;
    public ShieldBarScript shieldBar;
    public Image xpBackground;
    BoxCollider2D bx;


    void Start() {
        bx = GetComponent<BoxCollider2D>();
        //xpBackground.fillAmount = xp / xpNeeded;
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
        

        if(level == 0) {
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
    void Levelup() {
        levelText.text = "Level: " + (level + 2);
        healthBar.SetMaxHealth(levelHealth[level]);
        shieldBar.SetMaxShield(levelShield[level]);

    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "XP") {
            xp += 40;
            print(xp);
            Destroy(collision.gameObject);
        }
    }
}
