using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D col)

	{
         {
 
          
            if (ShieldBarScript.currentShield < 0)
            {
                HealthBar.currentHealth -= 10f;
            }
            else
            {
                ShieldBarScript.currentShield -= 30f;
            }
            
        }
    }
}
