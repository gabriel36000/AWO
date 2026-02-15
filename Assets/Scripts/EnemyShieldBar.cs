using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShieldBar : MonoBehaviour {

    public Image shieldFill; // drag in the green fill Image
    public Enemy enemy;      // your enemy script with shield values
    public Transform enemy1;
    public Vector3 offset;


    void Update() {
        float shieldPercent = (float)enemy.currentShield / enemy.maxShield;
        shieldFill.fillAmount = shieldPercent;
        if (enemy != null)
            transform.position = enemy1.position + offset;
    }
    void LateUpdate() {
        transform.rotation = Quaternion.identity;
    }
}
