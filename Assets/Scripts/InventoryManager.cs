using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] EqupmentPanel equpmentPanel;

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
            }
            else {
                inventory.AddItem(item);
            }
        }
    }
    public void Unequip(EquippableItem item) {
        if(!inventory.IsFull() && equpmentPanel.RemoveItem(item)) {
            inventory.AddItem(item);
        }
    }
    
}
