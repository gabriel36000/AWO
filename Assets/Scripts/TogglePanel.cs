using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TogglePanel : MonoBehaviour
{
    public GameObject panel;



    // Update is called once per frame
   public void Toggle()
    {
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf); // Toggles the active state
        }
    }
}
