using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Camerafollows : NetworkBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;

    public Vector3 offset;


	void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
