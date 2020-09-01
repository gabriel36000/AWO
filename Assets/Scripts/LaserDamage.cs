using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D col)
	{
		ShieldBarScript.Shield -= 30f;
	}
}
