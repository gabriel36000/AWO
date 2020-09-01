using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBoosters : MonoBehaviour {
	public float RotateSpeed = 30f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftArrow))
			transform.Rotate(-Vector3.up * RotateSpeed * Time.deltaTime);
		else if (Input.GetKey(KeyCode.RightArrow))
			transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime);
	}
}
