using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FriendlyLaserDamage : MonoBehaviour
{
    public int damage;
    private Friendly friendly;
    public GameObject PopUpPreFab;
    public GameObject damageEffect;
    public float criticalMultiplier = 2f;

    public void Setup(Friendly owner) // << New
    {
        friendly = owner;
        damage = friendly.damage;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (friendly == null)
        {
            Debug.LogWarning("Friendly reference missing on laser!");
            Destroy(gameObject);
            return;
        }

        if (col.CompareTag("Enemy") || col.CompareTag("Boss"))
        {
            int finalDamage = damage;

            int randomRoll = Random.Range(0, 100);
            if (randomRoll < friendly.criticalChance)
            {
                finalDamage = Mathf.RoundToInt(damage * criticalMultiplier);

                GameObject critPopUp = Instantiate(PopUpPreFab, transform.position, Quaternion.identity);
                if (critPopUp.transform.childCount > 0)
                {
                    var textMesh = critPopUp.transform.GetChild(0).GetComponent<TextMeshPro>();
                    if (textMesh != null)
                        textMesh.text = "CRIT! " + finalDamage.ToString();
                }
                Destroy(critPopUp, 0.7f);
            }
            else
            {
                GameObject popUpDamage = Instantiate(PopUpPreFab, transform.position, Quaternion.identity);
                if (popUpDamage.transform.childCount > 0)
                {
                    var textMesh = popUpDamage.transform.GetChild(0).GetComponent<TextMeshPro>();
                    if (textMesh != null)
                        textMesh.text = finalDamage.ToString();
                }
                Destroy(popUpDamage, 0.7f);
            }

            col.GetComponent<Enemy>().currentHealth -= finalDamage;

            if (damageEffect != null)
            {
                GameObject instance = Instantiate(damageEffect, transform.position, transform.rotation);
                Destroy(instance, 1f);
            }

            Destroy(gameObject);
        }
    }
}