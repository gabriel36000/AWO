using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSystem : MonoBehaviour {
    int xp = 0;
    int level = 0;
    int[] xpNeeded = { 100, 200, 300, 400 };
    int[] levelHealth = { 10, 30, 50, 100 };
    int[] levelShield = { 100, 200, 300, 400 };
    public TextMeshPro levelText;
    public HealthBar healthBar;
    public ShieldBarScript shieldBar;
    BoxCollider2D bx;


    void Start() {
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
            xp += 20;
            print(xp);
            Destroy(collision.gameObject);
        }
    }
}
