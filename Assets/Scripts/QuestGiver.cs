using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class QuestGiver : MonoBehaviour
{
    [Header("Quest Data")]
    public List<Quest> questsOffered = new List<Quest>(); // ✅ NEW

    [Header("UI Panels")]
    public GameObject backgroundQuest;
    public Transform questListContent;
    public GameObject questEntryPrefab;

    [Header("Detail Panel")]
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI levelRequirementText;
    public TextMeshProUGUI objectivesText;
    public Button acceptButton;

    [Header("References")]
    public QuestManager questManager;
    public Player player;

    private Quest selectedQuest;

    // ✅ Wrapper method with no parameters
    public void OpenQuestPanel()
    {
        acceptButton.gameObject.SetActive(false);
        OpenQuestList(questsOffered);
    }

    public void OpenQuestList(List<Quest> quests)
    {
        backgroundQuest.SetActive(true);

        // Clear old entries
        foreach (Transform child in questListContent)
            Destroy(child.gameObject);

        // Populate list
        foreach (Quest quest in quests)
        {
            // Skip quests already accepted
            if (questManager.activeQuests.Exists(q => q.questName == quest.questName))
                continue;

            Quest localQuest = quest;

            GameObject entry = Instantiate(questEntryPrefab, questListContent);
            TextMeshProUGUI entryText = entry.GetComponentInChildren<TextMeshProUGUI>();
            entryText.text = localQuest.questName;

            Button button = entry.GetComponent<Button>();
            button.onClick.AddListener(() => ShowQuestDetails(localQuest));
        }
    }

    private void ShowQuestDetails(Quest quest)
    {
        selectedQuest = quest;

        questNameText.text = quest.questName;
        levelRequirementText.text = $"Level Requirement: {quest.levelRequirement}";

        string obj = "";
        foreach (var o in quest.objectives)
            obj += $"- {o.objectiveName}: {o.currentAmount}/{o.requiredAmount}\n";
        objectivesText.text = obj;

        LevelSystem levelSystem = player.GetComponent<LevelSystem>();
        bool canAccept = levelSystem != null && levelSystem.level >= quest.levelRequirement;

        acceptButton.interactable = canAccept;
        acceptButton.onClick.RemoveAllListeners();

        if (canAccept)
        {
            acceptButton.gameObject.SetActive(true); // ✅ Show it only if level is OK
            acceptButton.onClick.RemoveAllListeners();
            acceptButton.onClick.AddListener(() =>
            {
                questManager.AcceptQuest(quest);
                OpenQuestList(questsOffered); // ✅ Refresh the list so the accepted one disappears
            });
        }
        else
        {
            acceptButton.gameObject.SetActive(false); // ❌ Hide if level too low
        }
    }

    public void ClosePanel()
    {
        backgroundQuest.SetActive(false);
    }

}

