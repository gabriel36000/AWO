using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType {
    weapon1,
    weapon2,
    engines,
    shield,
    reactor,
    
}
[CreateAssetMenu]
public class EquippableItem : Item
{
    [Header("Stats")]
    public int damage;
    public int health;
    public int shield;
    public int critChance;
    public int speed;
    public int rateOfFire;

    [Header("Ammo Settings")]
    public int maxAmmo;
    [HideInInspector]
    public int currentAmmo;

    public EquipmentType equipmentType;

    public void Equip(InventoryManager c)
    {
        Player player = GameObject.FindObjectOfType<Player>();
        PlayerMovement movement = GameObject.FindObjectOfType<PlayerMovement>();

        if (player != null)
        {
            player.maxHealth += health;
            player.maxShield += shield;
            player.minDamage += damage;
            player.maxDamage += damage;
            player.criticalChance += critChance;
        }

        if (movement != null)
        {
            movement.maxSpeed += speed;
        }
    }

    public void Unequip(InventoryManager c)
    {
        Player player = GameObject.FindObjectOfType<Player>();
        PlayerMovement movement = GameObject.FindObjectOfType<PlayerMovement>();

        if (player != null)
        {
            player.maxHealth -= health;
            player.maxShield -= shield;
            player.minDamage -= damage;
            player.maxDamage -= damage;
            player.criticalChance -= critChance;
        }

        if (movement != null)
        {
            movement.maxSpeed -= speed;
        }
    }
}