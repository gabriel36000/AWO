using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopScript : MonoBehaviour
{
    public int Credit;
    public int Health;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI healthText;
    public PlayerMoney playerMoney;
    public Inventory playerInventory;
    public AmmoItem laserAmmoPrefab;
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject Inventory = GameObject.FindWithTag("Inventory");
        if (player != null)
        {
            playerMoney = player.GetComponent<PlayerMoney>();
            playerInventory = Inventory.GetComponentInChildren<Inventory>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyLaserAmmo()
    {
        int cost = 2;

        if (playerMoney.SpendMoney(cost))
        {
            AmmoItem newAmmo = ScriptableObject.Instantiate(laserAmmoPrefab);
            newAmmo.currentStack = newAmmo.amountPerPickup;

            if (playerInventory.AddItem(newAmmo))
            {
                int laserAmmo = playerInventory.GetTotalAmmo("Laser Ammo");
                Debug.Log("Bought Laser Ammo. Total now: " + laserAmmo);
            }
        }
    }
    void UpdateCoinUI()
    {
        if (coinText != null && playerMoney != null)
        {
            coinText.text = playerMoney.GetMoney().ToString();
        }
    }
}
