using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    ShieldBarScript Shield1;

    [SerializeField] Inventory inventory;
    [SerializeField] EqupmentPanel equpmentPanel;
    private void Start() {
        Shield1 = FindObjectOfType<ShieldBarScript>();
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
            inventory.AddItem(item);
        }
    }
    
}
