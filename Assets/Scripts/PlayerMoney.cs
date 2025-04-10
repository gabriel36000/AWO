using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerMoney : MonoBehaviour
{
    public int money;
    public TextMeshProUGUI moneyUI;

    void Start()
    {
        money = 0;
        UpdateMoneyUI();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateMoneyUI();
            return true;
        }

        return false;
    }

    public int GetMoney()
    {
        return money;
    }

    private void UpdateMoneyUI()
    {
        if (moneyUI != null)
            moneyUI.text = "Money: " + money.ToString();
    }
    void Update()
    {
        UpdateMoneyUI();
    }
}

