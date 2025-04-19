using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{

    public Image healthFill; // drag in the green fill Image
    public Enemy enemy;      // your enemy script with health values
    public Transform enemy1;
    public Vector3 offset;


void Update()
    {
        float healthPercent = (float)enemy.currentHealth / enemy.maxHealth;
        healthFill.fillAmount = healthPercent;
        if (enemy != null)
            transform.position = enemy1.position + offset;
    }
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
