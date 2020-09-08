using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamageEnemy : MonoBehaviour
{
    public int damage;
    
    void OnTriggerEnter2D(Collider2D col) {
        
        if (col.transform.tag == "Player") {
            if(ShieldBarScript.currentShield > 0) {
                ShieldBarScript.currentShield -= damage;
            }
            else {
                HealthBar.currentHealth -= damage;
            }
            
            Destroy(gameObject);
        }
    }
}
