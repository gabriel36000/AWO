using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestEntryUI : MonoBehaviour
{
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI objectiveText;

    public void Setup(Quest quest)
    {
        questNameText.text = quest.questName + (quest.isCompleted ? " <color=green>(Completed)</color>" : " <color=yellow>(In Progress)</color>");

        string objectiveDetails = "";
        foreach (var obj in quest.objectives)
        {
            objectiveDetails += $"- {obj.objectiveName}: {obj.currentAmount}/{obj.requiredAmount}\n";
        }

        objectiveText.text = objectiveDetails;
    }
}

