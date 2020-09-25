using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUV : MonoBehaviour
{
    public float parrelax = 2f;

     void Update() {
        MeshRenderer mr = GetComponent<MeshRenderer>();

        Material mat = mr.material;
        
        Vector2 offset = mat.mainTextureOffset;

        offset.x = transform.position.x / transform.localScale.x / parrelax;
        offset.y = transform.position.y / transform.localScale.y / parrelax;

        mat.mainTextureOffset = offset;
    }
}
