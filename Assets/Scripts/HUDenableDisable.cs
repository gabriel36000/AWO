using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDenableDisable : MonoBehaviour
{
    public GameObject missionHUDPanel; // Drag your "MissionHUD" panel here
    public KeyCode toggleKey = KeyCode.M; // You can change to whatever key you want

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (missionHUDPanel != null)
            {
                missionHUDPanel.SetActive(!missionHUDPanel.activeSelf);
            }
        }
    }
}

