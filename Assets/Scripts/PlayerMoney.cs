using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerMoney : MonoBehaviour
{
    public int money;
    public TextMeshProUGUI moneyUI;

     void Start() {
        money = 0;
        moneyUI.text = "Money: " + money.ToString();
    }
     void Update() {
        moneyUI.text = "Money: " + money.ToString();
    }



}
