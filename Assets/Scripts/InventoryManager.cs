using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    Player health1;
    PlayerMovement movement1;

    [SerializeField] Inventory inventory;
    [SerializeField] EqupmentPanel equpmentPanel;
    [SerializeField] private AmmoItem ammoItemScriptableObject; // Drag your AmmoItem ScriptableObject in the inspector

    private void Start()
    {

        movement1 = FindObjectOfType<PlayerMovement>();
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

    public void Equip(EquippableItem item)
    {
        if (inventory.RemoveItem(item))
        {
            EquippableItem previousItem;
            if (equpmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(Instantiate(previousItem));
                    UnequipStats(previousItem); // 💥 important: remove old stats
                }

                EquipStats(item); // 💥 apply new stats

                item.Equip(this);
            }
            else
            {
                inventory.AddItem(Instantiate(item));
            }
        }
    }

    public void Unequip(EquippableItem item)
    {
        if (!inventory.IsFull() && equpmentPanel.RemoveItem(item))
        {
            UnequipStats(item); // 💥 remove stats
            inventory.AddItem(Instantiate(item));
        }
    }
    private void EquipStats(EquippableItem item)
    {
        if (item == null) return;

        if (health1 != null)
        {
            health1.maxHealth += item.health;
            health1.maxShield += item.shield;
            health1.minDamage += item.damage;
            health1.maxDamage += item.damage;
            health1.criticalChance += item.critChance;

            // Track bonuses
            health1.bonusHealth += item.health;
            health1.bonusShield += item.shield;
            health1.bonusDamage += item.damage;
            health1.bonusCritChance += item.critChance;
        }

        if (movement1 != null)
        {
            movement1.maxSpeed += item.speed;
            health1.bonusSpeed += item.speed;
        }

        health1.UpdateHealthBar();
        health1.UpdateShieldBar();
    }

    private void UnequipStats(EquippableItem item)
    {
        if (item == null) return;

        if (health1 != null)
        {
            health1.maxHealth -= item.health;
            health1.maxShield -= item.shield;
            health1.minDamage -= item.damage;
            health1.maxDamage -= item.damage;
            health1.criticalChance -= item.critChance;

            // Remove bonuses
            health1.bonusHealth -= item.health;
            health1.bonusShield -= item.shield;
            health1.bonusDamage -= item.damage;
            health1.bonusCritChance -= item.critChance;
        }

        if (movement1 != null)
        {
            movement1.maxSpeed -= item.speed;
            health1.bonusSpeed -= item.speed;
        }

        health1.UpdateHealthBar();
        health1.UpdateShieldBar();
    }
}
