﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class projectiles : MonoBehaviour {
	private float projectileVelocity;
	private List<GameObject> Projectiles = new List<GameObject>();
	public GameObject projectilePrefab;
    public AudioClip laserSound;
    public float fireRate = 0.5f;
    private float nextFire = 0.0f;
	void Start () {
		projectileVelocity = 0;
	}


	void Update() {
		if (Input.GetMouseButtonDown(0) && Time.time > nextFire) {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            nextFire = Time.time + fireRate;
			GameObject bullet = (GameObject)Instantiate(projectilePrefab, transform.position, transform.rotation);
			Projectiles.Add(bullet);
            AudioSource.PlayClipAtPoint(laserSound, transform.position);
		}
		for (int i = 0; i <Projectiles.Count; i++) {
			GameObject goBullet = Projectiles[i];
			if (goBullet != null) {
				Vector3 offset = transform.rotation * new Vector3(0, 3.5f, 0);
				goBullet.transform.Translate(new Vector3(0, 0) * Time.deltaTime * projectileVelocity);
				

				Vector3 bulletScreenpos = Camera.main.WorldToScreenPoint(goBullet.transform.position);
				if (bulletScreenpos.y >= Screen.height) {
					DestroyObject(goBullet);
					Projectiles.Remove(goBullet);
				}
			}
		}
	}
}
