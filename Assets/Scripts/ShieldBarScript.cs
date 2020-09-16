using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShieldBarScript : MonoBehaviour {
	Image ShieldBarColor;
	public int maxShield = 1000;
	public int currentShield;
    public TextMeshProUGUI shield;
    public GameObject bubbleShield;
    public GameObject lighting;
    public GameObject lightingShieldDown;
    private float regenrate = 20f;
    public float lastTime;
    


	// Use this for initialization
	void Start () {
		ShieldBarColor = GetComponent<Image> ();
		currentShield = maxShield;
        lastTime = Time.time;

	}
	
    public void SetMaxShield(int shield) {
        maxShield += shield;
        currentShield = maxShield;
    }
	// Update is called once per frame
	void FixedUpdate () {
		ShieldBarColor.fillAmount = (float)currentShield / maxShield;
        shield.text = "Shield:" + (currentShield);
        effectShield();
        regenerating();
        
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

    public void regenerating() {
        if((Time.time > lastTime + regenrate) && (currentShield < maxShield)) {
            currentShield += maxShield / 10;
            lastTime = Time.time;
        }
    }

}

