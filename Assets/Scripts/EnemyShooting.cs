﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Vector3 bulletoffset = new Vector3(0, 0.5f, 0);
    public Vector3 rotationoffset = new Vector3(0, 0, 0);

    public GameObject bulletPrefab;

    public float attackRange;
    public Transform player;
    public GameObject player1;
    
    


    public float fireDelay = 0.50f;
    float cooldownTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("Player");
        player = player1.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() {
        
        if (Vector2.Distance(player.position, transform.position) <= attackRange) {


            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer < -0) {
                cooldownTimer = fireDelay;

                Vector3 offset = transform.rotation * bulletoffset;
                GameObject bulletGo = (GameObject)Instantiate(bulletPrefab, transform.position + offset, Quaternion.Euler(rotationoffset) * transform.rotation);
                
            }
        }
        
    }
   
}
