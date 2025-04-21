using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class QuestManager : MonoBehaviour
{
    [Header("Quest Setup")]
    public List<Quest> startingQuests; // Drag ScriptableObject quests here
    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();

    [Header("UI (Optional)")]
    public TextMeshProUGUI questLogText; // Drag a UI Text element here (optional)


    public void UpdateObjective(string killedEnemyType)
    {
        List<Quest> questsToComplete = new List<Quest>();

        foreach (var quest in activeQuests)
        {
            foreach (var obj in quest.objectives)
            {
                if (!obj.IsComplete && obj.targetEnemyType == killedEnemyType)
                {
                    obj.currentAmount++;
                    Debug.Log($"Updated: {obj.objectiveName} ({obj.currentAmount}/{obj.requiredAmount})");
                }
            }

            if (quest.objectives.TrueForAll(o => o.IsComplete) && !quest.isCompleted)
            {
                quest.isCompleted = true;
                Debug.Log($"Quest Complete: {quest.questName}");

                GiveReward(quest); // ✅ Give rewards

                questsToComplete.Add(quest); // ✅ Mark for moving after loop
            }
        }

        // ✅ Move completed quests after looping
        foreach (var completed in questsToComplete)
        {
            activeQuests.Remove(completed);
            completedQuests.Add(completed);
        }

        RefreshQuestLog();
    }

    public void RefreshQuestLog()
    {
        if (questLogText == null) return;

        string log = "";
        foreach (var quest in activeQuests)
        {
            log += $"{quest.questName} - {(quest.isCompleted ? "✔ Completed" : "In Progress")}\n";
            foreach (var obj in quest.objectives)
            {
                log += $"- {obj.objectiveName}: {obj.currentAmount}/{obj.requiredAmount}\n";
            }
        }

        questLogText.text = log;
    }
    public void GiveReward(Quest quest)
    {
        Player player = FindObjectOfType<Player>();
        PlayerMoney money = player.GetComponent<PlayerMoney>();

        if (player != null)
        {
            player.GainXP(quest.rewardXP); // ✅ Now works through LevelSystem
        }

        if (money != null)
        {
            money.AddMoney(quest.rewardCredits); // ✅ This adds gold to current money
        }


        Debug.Log($"Reward given: {quest.rewardXP} XP, {quest.rewardCredits} Credits");
    }
    public void AcceptQuest(Quest questTemplate)
    {
        Player player = FindObjectOfType<Player>();
        if (player == null) return;

        LevelSystem levelSystem = player.GetComponent<LevelSystem>();
        if (levelSystem == null) return;

        // Check if quest already active
        if (activeQuests.Any(q => q.questName == questTemplate.questName))
        {
            Debug.Log($"Quest '{questTemplate.questName}' is already active.");
            return;
        }

        // Check level requirement
        if (levelSystem.level < questTemplate.levelRequirement)
        {
            Debug.Log($"Cannot accept quest '{questTemplate.questName}' — requires level {questTemplate.levelRequirement}.");
            return;
        }

        // Accept it
        Quest questInstance = Instantiate(questTemplate);
        activeQuests.Add(questInstance);
        Debug.Log($"Accepted Quest: {questInstance.questName}");
        RefreshQuestLog();
    }
}
