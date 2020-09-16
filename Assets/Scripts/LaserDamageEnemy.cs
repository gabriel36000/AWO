using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamageEnemy : MonoBehaviour
{
    public int damage;
    ShieldBarScript shield;
    HealthBar health;
    public GameObject ShieldBarColor;
    public GameObject HealthBarColor;
    

     void Start() {
        ShieldBarColor = GameObject.Find("ShieldBarColor");
        HealthBarColor = GameObject.Find("HealthBarColor");
        shield = ShieldBarColor.GetComponent<ShieldBarScript>();
        health = HealthBarColor.GetComponent<HealthBar>();

        
    }
    void OnTriggerEnter2D(Collider2D col) {
        
        if (col.transform.tag == "Player") {
            if(shield.currentShield > 0) {
                shield.currentShield -= damage;
                shield.lastTime = Time.time;
            }
            else {
                health.currentHealth -= damage;
                shield.lastTime = Time.time;
            }
            shield.lastTime = Time.time;
            Destroy(gameObject);
        }
    }
}
