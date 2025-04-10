using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/AmmoItem")]
public class AmmoItem : Item
{
    public int amountPerPickup = 30;

    private void OnEnable()
    {
        isStackable = true;

        // Ensure the stack is initialized to at least 1 or whatever default
        if (currentStack <= 0)
            currentStack = amountPerPickup;
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Tools/Reset Ammo Stack")]
    static void ResetAmmo()
    {
        var ammo = UnityEditor.Selection.activeObject as AmmoItem;
        if (ammo != null)
        {
            ammo.currentStack = 0;
            UnityEditor.EditorUtility.SetDirty(ammo);
            Debug.Log("Ammo stack reset!");
        }
    }
#endif
}



