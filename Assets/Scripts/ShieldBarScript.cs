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
    public GameObject bubbleShield;
    public GameObject lighting;
    public GameObject lightingShieldDown;
    


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
        effectShield();
        
	}
    public void effectShield() {
        if (currentShield <= 0) {
            bubbleShield.SetActive(false);
            lighting.SetActive(false);
            lightingShieldDown.SetActive(true);
        }
        else {
            bubbleShield.SetActive(true);
            lighting.SetActive(true);
            lightingShieldDown.SetActive(false);
        }
    }
}

