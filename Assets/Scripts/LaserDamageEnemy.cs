using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaserDamageEnemy : MonoBehaviour
{
    public int damage;
    ShieldBarScript shield;
    HealthBar health;
    public GameObject ShieldBarColor;
    public GameObject HealthBarColor;
    public GameObject damageEffect;
    public GameObject PopUpPreFab;

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
                damageEffect.SetActive(true);
                Instantiate(damageEffect, transform.position, transform.rotation);
                GameObject PopUpDamage = Instantiate(PopUpPreFab, transform.position, Quaternion.identity);
                PopUpDamage.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();

                Destroy(PopUpDamage, 0.7f);
            }
            else {
                health.currentHealth -= damage;
                shield.lastTime = Time.time;
                damageEffect.SetActive(true);
                Instantiate(damageEffect, transform.position, transform.rotation);
                GameObject PopUpDamage = Instantiate(PopUpPreFab, transform.position, Quaternion.identity);
                PopUpDamage.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();

                Destroy(PopUpDamage, 0.7f);
            }
            shield.lastTime = Time.time;
            Destroy(gameObject);
        }
    }
    public void Update() {
        Destroy(gameObject, 3);
    }
}
