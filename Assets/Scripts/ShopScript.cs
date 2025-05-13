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
    public GameObject friendlyPrefab;
    public Transform player;
    public float formationSpacing = 2.5f;
    public Player player1;


    private List<GameObject> hiredShips = new List<GameObject>();
    void Start()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        GameObject inventoryGO = GameObject.FindWithTag("Inventory");

        if (playerGO != null)
        {
            playerMoney = playerGO.GetComponent<PlayerMoney>();
            playerInventory = inventoryGO.GetComponentInChildren<Inventory>();

            player = playerGO.transform; // ✅ assign to your public field
            player1 = playerGO.GetComponent<Player>(); // ✅ assign the Player script
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
    public void HireShip()
    {
        int cost = 100;

        if (player1.hiredFriendlies.Count >= player1.maxFriendlyShip)
        {
            Debug.Log("Max friendly ships reached.");
            return;
        }

        if (playerMoney.SpendMoney(cost))
        {
            Vector3 spawnPos = player.transform.position + Vector3.right * 2;
            GameObject newShip = Instantiate(friendlyPrefab, spawnPos, Quaternion.identity);

            Friendly friendlyScript = newShip.GetComponent<Friendly>();
            friendlyScript.isHired = true;
            friendlyScript.followTarget = player.transform;
            friendlyScript.formationOffset = new Vector2(2f * player1.hiredFriendlies.Count, -2f);

            player1.hiredFriendlies.Add(newShip);
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
