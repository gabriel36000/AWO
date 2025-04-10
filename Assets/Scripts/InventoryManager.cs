using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    ShieldBarScript Shield1;
    Player health1;
    PlayerMovement movement1;

    [SerializeField] Inventory inventory;
    [SerializeField] EqupmentPanel equpmentPanel;
    [SerializeField] private AmmoItem ammoItemScriptableObject; // Drag your AmmoItem ScriptableObject in the inspector

    private void Start()
    {

        movement1 = FindObjectOfType<PlayerMovement>();
        Shield1 = FindObjectOfType<ShieldBarScript>();
        health1 = FindObjectOfType<Player>();
        inventory.Items.RemoveAll(item => item is AmmoItem);
        // Always instantiate and then set stack manually
        AmmoItem newAmmo = Instantiate(ammoItemScriptableObject); // clone the SO
        newAmmo.currentStack = 120; // reset the clone's stack

        inventory.AddItem(newAmmo);
    }

    private void Awake() {
        inventory.OnItemRightClickEvent += EquipFromInventory;
        equpmentPanel.OnItemRightClickEvent += UnequipFromEquipPanel;
    }
    private void UnequipFromEquipPanel(Item item) {
        if (item is EquippableItem) {
            Unequip((EquippableItem)item);
        }
    }
    private void EquipFromInventory(Item item) {
        if(item is EquippableItem) {
            Equip((EquippableItem)item);
        }
    }

    public void Equip(EquippableItem item) {
        if (inventory.RemoveItem(item)) {
            EquippableItem previousItem;
            if(equpmentPanel.AddItem(item, out previousItem)) {
                if(previousItem != null) {
                    inventory.AddItem(previousItem);
                    
                }
                item.Equip(this);
                Shield1.maxShield += item.shield;
                health1.maxHealth += item.health;
                movement1.maxSpeed += item.speed;
                Debug.Log("Hello");

            }

            else {
                
                inventory.AddItem(item);
            }
        }
    }
    public void Unequip(EquippableItem item) {
        if(!inventory.IsFull() && equpmentPanel.RemoveItem(item)) {
            Shield1.maxShield -= item.shield;
            health1.maxHealth -= item.health;
            movement1.maxSpeed -= item.speed;
            inventory.AddItem(item);
        }
    }
    
}
