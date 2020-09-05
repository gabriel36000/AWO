﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    void LateUpdate()
    {
		if (Camera.main) {
			transform.rotation = Camera.main.transform.rotation;
		}
	}
}
