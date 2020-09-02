using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (HealthBar.currentHealth <= 0)
        {
            Debug.Log("Death");
            Destroy(gameObject);
        }
             
    }
}

