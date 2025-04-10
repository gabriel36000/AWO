using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class Item : ScriptableObject
{
    public Sprite Icon;
    public string itemName;

    [Header("Stacking")]
    public bool isStackable;
    public int maxStack = 999;
    [HideInInspector]
    public int currentStack = 1;
}
