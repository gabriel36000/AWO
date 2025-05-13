using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D myPlayer;
    [SerializeField]
    private float movementSpeed;
    public float turnSpeed;
    public float SpeedBoost;
    public float maxSpeed;
    public float acceleration;
    public float MaxBoostspeed;


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        float horizotal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float Boost = Input.GetAxis("Boost");

        transform.Rotate(0f, 0f, horizotal * Time.deltaTime * turnSpeed);
        transform.Translate(0f, Boost * Time.deltaTime * SpeedBoost, 0f);


        if (vertical > 0)
        {
            movementSpeed = Mathf.Lerp(movementSpeed, maxSpeed, 0.01f);

        }
        else
        {
            movementSpeed = Mathf.Lerp(movementSpeed, 0f, 0.01f);
        }

        transform.Translate(0f, 1f * Time.deltaTime * movementSpeed, 0f);

        if (Boost > 0)
        {
            SpeedBoost = Mathf.Lerp(Boost, MaxBoostspeed, 0.01f);
        }
        else
        {
            SpeedBoost = Mathf.Lerp(Boost, 0f, 0.01f);
        }


    }




}