using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject uiPopUp;

    void OnTriggerEnter2D(Collider2D col) {
        if (col.transform.tag == "Player") {
            uiPopUp.SetActive(true);
        }
       
    }

     void OnTriggerExit2D(Collider2D col) {
        if(col.transform.tag == "Player") {
            uiPopUp.SetActive(false);
        }
        
    }




}





