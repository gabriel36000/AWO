﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Enemy : MonoBehaviour {
    public float speed;
    public float maxSpeed;
    public float stoppingDistance;
    public float rotationSpeed;
    private Transform target;
    public GameObject player;
    public int currentHealth; 
    public int maxHealth;
    public int xpValue;
    LevelSystem levelSystem;            //store scripts 
    public GameObject explosion;
    public TextMeshPro enemyName;
    public Image enemyHealth;
    public int bulletDamage;
    public float gotoRange;
    public Transform player1;
    public GameObject[] waypoints;
    private GameObject currentWaypoint;
    private int wpIndex = 0;
    Rigidbody2D rigidbody;
    Animator damageAnim;
    Player player2;
    public int moneyValueLow, moneyValueHigh;
    PlayerMoney money;
    public GameObject popUpPreFabMoney;
    
   
    






    void Start() {
       
        player = GameObject.Find("Player");
        player1 = player.GetComponent<Transform>();
        target = player.GetComponent<Transform>();
        currentHealth = maxHealth;
        levelSystem = player.GetComponent<LevelSystem>(); //access a public variable from different script
        money = player.GetComponent<PlayerMoney>();
        wpIndex = 0;
        currentWaypoint = waypoints[wpIndex];
        rigidbody = GetComponent<Rigidbody2D>();
        
        




    }

    // Update is called once per frame
    void Update() {
        enemyHealth.fillAmount = (float)currentHealth / maxHealth;

        if(Vector2.Distance(player1.position, transform.position) <= gotoRange){
            go();
        }
        else {
            patrol();
        }
        
        death();

    }
    private void RotateTowards(Vector2 target) {
        var offset = 270f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private void death() {
        if (currentHealth <= 0) {

            levelSystem.xp += xpValue;
            money.money += UnityEngine.Random.Range(moneyValueLow, moneyValueHigh);
            print("money" + money.money);
            GameObject PopUpmoney = Instantiate(popUpPreFabMoney, transform.position, Quaternion.identity);
            PopUpmoney.transform.GetChild(0).GetComponent<TextMeshPro>().text = "money: " + money.money.ToString ();

            Destroy(PopUpmoney, 2.7f);
            explosion.SetActive(true);
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);




        }
    }
    private void go(){
        float distance = Vector2.Distance(player1.position, transform.position);
        RotateTowards(target.position);


        if (distance > stoppingDistance)
        {
           
            speed = Mathf.Lerp(speed, maxSpeed, 0.01f); // Gradually accelerate
            rigidbody.velocity = transform.up * speed;
        }
        // If the enemy is within the stopping distance, stop its movement
        else
        {
            speed = Mathf.Lerp(speed, 0f, 0.015f);
            rigidbody.velocity = transform.up * speed;

        }
    }
    private void patrol() {
        if (WpReached(transform.position, currentWaypoint.transform.position, 5)) {
            UpdatedWp(); // move to next waypoint if reached the current one
        }

        RotateTowards(currentWaypoint.transform.position);
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, currentWaypoint.transform.position, speed * Time.deltaTime); // Move to current waypoint
        speed = Mathf.Lerp(speed, maxSpeed, 0.01f);
        

    }
    private bool WpReached(Vector2 position, Vector2 target, float allownace) {
        return Vector2.Distance(position, target) <= allownace;
    }
    private void UpdatedWp() {
        print("Updated Wp");
        wpIndex++; // Next waypoint
        wpIndex = wpIndex % waypoints.Length; // Cycling from 0 to waypoint length
        currentWaypoint = waypoints[wpIndex];
    }


}
