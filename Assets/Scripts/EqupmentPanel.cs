using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EqupmentPanel : MonoBehaviour
{
    [SerializeField] Transform equipmentSlotParent;
    [SerializeField] EquipmentSlots[] equipmentSlots;


    public event Action<Item> OnItemRightClickEvent;

    private void Awake() {
        for (int i = 0; i < equipmentSlots.Length; i++) {
            equipmentSlots[i].OnRightClickEvent += OnItemRightClickEvent;
        }
    }

    private void OnValidate() {
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<EquipmentSlots>();
    }
    public bool AddItem(EquippableItem item, out EquippableItem previousItem) {
        for(int i = 0; i < equipmentSlots.Length; i++) {
            if(equipmentSlots[i].EquipmentType == item.equipmentType) {
                previousItem = (EquippableItem)equipmentSlots[i].Item;
                equipmentSlots[i].Item = item;
                return true;
            }
        }
        previousItem = null;
        return false;
    }
    public bool RemoveItem(EquippableItem item) {
        for (int i = 0; i < equipmentSlots.Length; i++) {
            if (equipmentSlots[i].Item == item) {
                equipmentSlots[i].Item = null;
                return true;
            }
        }
        return false;
    }
}
