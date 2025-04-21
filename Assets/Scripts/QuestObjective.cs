using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class QuestObjective
{
    public string objectiveName; 
    public string targetEnemyType; 
    public int requiredAmount;
    public int currentAmount;

    public bool IsComplete => currentAmount >= requiredAmount;
}
