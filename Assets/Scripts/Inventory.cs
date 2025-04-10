using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items;
    [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlot[] itemSlots;

    public event Action<Item> OnItemRightClickEvent;
    public List<Item> Items => items;
    private void Awake()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].OnRightClickEvent += OnItemRightClickEvent;
        }
    }


    private void OnValidate()
    {
        if (itemsParent != null)
        {
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();

            RefreshUI();
        }
    }
    private void RefreshUI()
    {
        int i = 0;
        for (; i < items.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = items[i];
        }
        for (; i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
        }
    }
    public bool AddItem(Item item)
    {
        if (item.isStackable)
        {
            foreach (Item i in items)
            {
                if (i.itemName == item.itemName && i.currentStack < i.maxStack)
                {
                    i.currentStack += item.currentStack;
                    RefreshUI();
                    return true;
                }
            }
        }

        if (IsFull())
            return false;

        items.Add(item);
        RefreshUI();
        return true;
    }
    public bool RemoveItem(Item item)
    {
        if (items.Remove(item))
        {
            RefreshUI();
            return true;
        }
        return false;
    }
    public bool IsFull()
    {
        return items.Count >= itemSlots.Length;
    }
    public int GetTotalAmmo(string ammoName)
    {
        int total = 0;
        foreach (var item in items)
        {
            if (item is AmmoItem ammo && ammo.itemName == ammoName)
            {
                total += ammo.currentStack;
            }
        }
        return total;
    }

    public bool ConsumeAmmo(string ammoName, int amount)
    {
        int remaining = amount;
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i] is AmmoItem ammoItem && ammoItem.itemName == ammoName && ammoItem.currentStack > 0)
            {
                int consume = Mathf.Min(remaining, ammoItem.currentStack);
                ammoItem.currentStack -= consume;
                remaining -= consume;

                if (ammoItem.currentStack <= 0)
                    items.RemoveAt(i);

                if (remaining <= 0)
                    return true;
            }
        }

        return false;
    }

}

