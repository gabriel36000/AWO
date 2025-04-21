using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/New Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string description;
    public bool isCompleted;

    public int rewardXP;
    public int rewardCredits;
    public int levelRequirement;

    public List<QuestObjective> objectives;
}
