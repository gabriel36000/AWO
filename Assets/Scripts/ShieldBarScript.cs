using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShieldBarScript : MonoBehaviour {
	Image ShieldBarColor;
	public int maxShield = 1000;
	public static float currentShield;
    public TextMeshProUGUI shield;


	// Use this for initialization
	void Start () {
		ShieldBarColor = GetComponent<Image> ();
		currentShield = maxShield;

	}
	
    public void SetMaxShield(int shield) {
        maxShield += shield;
        currentShield = maxShield;
    }
	// Update is called once per frame
	void FixedUpdate () {
		ShieldBarColor.fillAmount = currentShield / maxShield;
        shield.text = "Shield:" + (currentShield);

        
	}
}
