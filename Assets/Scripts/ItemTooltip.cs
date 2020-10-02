using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemSlot;
    [SerializeField] TextMeshProUGUI itemStatsText;

    private StringBuilder sb = new StringBuilder();

    public void ShowTooltip(EquippableItem item) {
        itemName.text = item.ItemName;
        itemSlot.text = item.equipmentType.ToString();

        sb.Length = 0;
        AddStat(item.shield, " Shield");
        AddStat(item.health, " Health");
        AddStat(item.damage, " damage");
        AddStat(item.speed,  " speed");

        itemStatsText.text = sb.ToString();




        gameObject.SetActive(true);
    }

    public void HideTooltip() {
        gameObject.SetActive(false);
    }

    private void AddStat(float value, string statName) {
        if(value != 0) {
            if (sb.Length > 0)
                sb.AppendLine();

            if (value > 0)
                sb.Append("+");

            sb.Append(value);
            sb.Append("");
            sb.Append(statName);
        }
    }
}
