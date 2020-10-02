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
    ShieldBarScript Shield1;
   
    public int damage;
    public int health;
    public int shield;
    public int critChance;
    public int speed;
    public int rateOfFire;
    [Space]
    public EquipmentType equipmentType;

    public void start() {
        Shield1 = FindObjectOfType<ShieldBarScript>();
        
    }

    public void Equip(InventoryManager c) {
        if(shield != 0) {
           
            
        }
                
    }


    public void Unequip(InventoryManager c) {
        
        
    }

    
}
