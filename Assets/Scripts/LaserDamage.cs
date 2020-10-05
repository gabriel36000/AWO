using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaserDamage : MonoBehaviour {
    public int damage;

    Player player;
    public GameObject player1;
    public GameObject PopUpPreFab;
    Enemy enemy;
    public GameObject damageEffect;
    

    
    


    public void Start() {
        player1 = GameObject.Find("Player");
        player = player1.GetComponent<Player>();
        damage = player.damage;
        
       
    }

    void OnTriggerEnter2D(Collider2D col) { 

         if(col.transform.tag == "Enemy") {
            col.GetComponent<Enemy>().currentHealth -= damage;
            damageEffect.SetActive(true);
            Instantiate(damageEffect, transform.position, transform.rotation);
            print(damage);
            print(transform.position);
            Destroy(gameObject);
            GameObject PopUpDamage = Instantiate(PopUpPreFab, transform.position, Quaternion.identity);
            PopUpDamage.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
            
            Destroy(PopUpDamage, 0.7f);
        }
    }
}
