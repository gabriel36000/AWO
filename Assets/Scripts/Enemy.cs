using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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





    void Start() {
        target = player.GetComponent<Transform>();
        currentHealth = maxHealth;
        levelSystem = player.GetComponent<LevelSystem>(); //access a public variable from different script




    }

    // Update is called once per frame
    void Update() {
        enemyHealth.fillAmount = (float)currentHealth / maxHealth;
        go();
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
            explosion.SetActive(true);
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);




        }
    }
    private void go(){
        if (Vector2.Distance(player1.position, transform.position) <= gotoRange) {
            RotateTowards(target.position);

            if (Vector2.Distance(transform.position, target.position) > stoppingDistance) {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime); // Move towards player
                speed = Mathf.Lerp(speed, maxSpeed, 0.01f); // Speed increase by 0.01 unitl maxSpeed
            }

            else {
                speed = Mathf.Lerp(speed, 0f, 0.01f);
            }
        }
    }
}
