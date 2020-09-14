using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamageEnemy : MonoBehaviour
{
    public int damage;
    ShieldBarScript shield;

     void Start() {
        shield = GetComponent<ShieldBarScript>();
    }
    void OnTriggerEnter2D(Collider2D col) {
        
        if (col.transform.tag == "Player") {
            if(ShieldBarScript.currentShield > 0) {
                ShieldBarScript.currentShield -= damage;
                shield.lastTime = Time.time;
            }
            else {
                HealthBar.currentHealth -= damage;
                shield.lastTime = Time.time;
            }
            shield.lastTime = Time.time;
            Destroy(gameObject);
        }
    }
}
