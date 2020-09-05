using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour {
    public int damage;
	void OnTriggerEnter2D(Collider2D col) { 

         if(col.transform.tag == "Enemy") {
            col.GetComponent<Enemy>().currentHealth -= damage;
            Destroy(gameObject);
         }
    }
}
