using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camerafollows : MonoBehaviour {

    public Transform target;

    public float smoothSpeed = 0.125f;
    
    public Vector3 offset;

    private void Start() {
        
    }

    void Update() {
        transform.position = target.position + offset;
        
    }
}
