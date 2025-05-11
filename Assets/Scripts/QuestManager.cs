using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class QuestManager : MonoBehaviour
{
    [Header("Quest Setup")]
    public List<Quest> startingQuests;
    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();

    [Header("UI Elements")]
    public GameObject questScreen; // Your full quest screen panel
    public Transform questHUDContent; // ➔ HUD panel (small one on main screen)
    public GameObject questHUDEntryPrefab;

    private List<GameObject> currentHUDEntries = new List<GameObject>(); // To clear old HUDs

    private void Start()
    {
        foreach (var quest in startingQuests)
        {
            AcceptQuest(quest);
        }

        if (questScreen != null)
        {
            questScreen.SetActive(false);
        }
        RefreshQuestHUD();
    }

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

                GiveReward(quest);
                questsToComplete.Add(quest);
            }
        }

        RefreshQuestHUD();

        foreach (var completed in questsToComplete)
        {
            activeQuests.Remove(completed);
            completedQuests.Add(completed);
        }// 🛠 Update HUD after progress
    }

    public void GiveReward(Quest quest)
    {
        Player player = FindObjectOfType<Player>();
        PlayerMoney money = player?.GetComponent<PlayerMoney>();

        if (player != null)
        {
            player.GainXP(quest.rewardXP);
        }

        if (money != null)
        {
            money.AddMoney(quest.rewardCredits);
        }

        Debug.Log($"Reward given: {quest.rewardXP} XP, {quest.rewardCredits} Credits");
    }

    public void AcceptQuest(Quest questTemplate)
    {
        Player player = FindObjectOfType<Player>();
        if (player == null) return;

        LevelSystem levelSystem = player.GetComponent<LevelSystem>();
        if (levelSystem == null) return;

        if (activeQuests.Any(q => q.questName == questTemplate.questName))
        {
            Debug.Log($"Quest '{questTemplate.questName}' is already active.");
            return;
        }

        if (levelSystem.level < questTemplate.levelRequirement)
        {
            Debug.Log($"Cannot accept quest '{questTemplate.questName}' — requires level {questTemplate.levelRequirement}.");
            return;
        }

        Quest questInstance = CloneQuest(questTemplate);
        activeQuests.Add(questInstance);
        Debug.Log($"Accepted Quest: {questInstance.questName}");

        QuestScreenUI questScreenUI = FindObjectOfType<QuestScreenUI>();
        if (questScreenUI != null && questScreen.activeSelf)
        {
            questScreenUI.RefreshQuestList();
        }

        RefreshQuestHUD(); // 🛠 Update HUD after new quest accepted
    }

    public void OpenQuestScreen()
    {
        if (questScreen != null)
        {
            questScreen.SetActive(true);

            QuestScreenUI questScreenUI = FindObjectOfType<QuestScreenUI>();
            if (questScreenUI != null)
            {
                questScreenUI.RefreshQuestList();
            }
        }
    }

    public void CloseQuestScreen()
    {
        if (questScreen != null)
        {
            questScreen.SetActive(false);
        }
    }

    // 🛠 NEW: Updates the quest info shown on the player's HUD
    private void RefreshQuestHUD()
    {
        if (questHUDContent == null) return;

        // Clear old HUD entries
        foreach (GameObject entry in currentHUDEntries)
        {
            Destroy(entry);
        }
        currentHUDEntries.Clear();

        // Create a single prefab entry for each active quest (name + objectives together)
        foreach (var quest in activeQuests)
        {
            GameObject entryObj = Instantiate(questHUDEntryPrefab, questHUDContent);
            QuestEntryUI entryUI = entryObj.GetComponent<QuestEntryUI>();

            if (entryUI != null)
            {
                entryUI.Setup(quest);
            }
            else
            {
                Debug.LogError("❌ questHUDEntryPrefab is missing the QuestEntryUI script.");
            }

            currentHUDEntries.Add(entryObj);
        }
    }
    private Quest CloneQuest(Quest template)
    {
        Quest newQuest = ScriptableObject.CreateInstance<Quest>();
        newQuest.questName = template.questName;
        newQuest.description = template.description;
        newQuest.rewardXP = template.rewardXP;
        newQuest.rewardCredits = template.rewardCredits;
        newQuest.levelRequirement = template.levelRequirement;
        newQuest.isCompleted = false;

        newQuest.objectives = new List<QuestObjective>();
        foreach (var obj in template.objectives)
        {
            QuestObjective newObj = new QuestObjective
            {
                objectiveName = obj.objectiveName,
                targetEnemyType = obj.targetEnemyType,
                requiredAmount = obj.requiredAmount,
                currentAmount = 0
            };
            newQuest.objectives.Add(newObj);
        }

        return newQuest;
    }
}