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
    public int damage;
    public int health;
    public int shield;
    public int critChance;
    public int speed;
    public int rateOfFire;
    [Space]
    public float damagePercentBonus;
    public float healthPercentBonus;
    public float shieldPercentBonus;
    public float critChancePercentBonus;
    public float speedPercentBonus;
    public float RateOfFirePercentBonus;
    [Space]
    public EquipmentType equipmentType;

}
