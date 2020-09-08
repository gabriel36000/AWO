using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorRotation : MonoBehaviour
{
    public int rotationSpeed; // How fast does the rotation go
    public float distance = 0.5f;  // Amount to move left and right from the start point
    public float speed = 0.3f; // How fast does the rock go
    private Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its local y axis at 1 degree per second
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        Vector3 v = startPos;
        v.x += distance * Mathf.Sin(Time.time * speed);
        transform.position = v;
    }
}
