using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendlyHealthBar : MonoBehaviour
{
    public Image healthFill; // drag in the green fill Image
    public Friendly friendly;
    public Transform friendly1;
    public Vector3 offset;


    void Update()
    {
        float healthPercent = (float)friendly.currentHealth / friendly.maxHealth;
        healthFill.fillAmount = healthPercent;
        if (friendly != null)
            transform.position = friendly1.position + offset;
    }
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
