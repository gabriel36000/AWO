using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBarScript : MonoBehaviour {
	Image ShieldBarColor;
	float maxShield = 100f;
	public static float Shield;


	// Use this for initialization
	void Start () {
		ShieldBarColor = GetComponent<Image> ();
		Shield = maxShield;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		ShieldBarColor.fillAmount = Shield / maxShield;
	}
}
