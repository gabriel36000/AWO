using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour {
    public int damage;

    Player player;
    public GameObject player1;

    public void Start() {
        player1 = GameObject.Find("Player");
        player = player1.GetComponent<Player>();
        damage = player.damage;
    }

    void OnTriggerEnter2D(Collider2D col) { 

         if(col.transform.tag == "Enemy") {
            col.GetComponent<Enemy>().currentHealth -= damage;
            print(damage);
            Destroy(gameObject);
         }
    }
}
