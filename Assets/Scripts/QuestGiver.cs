using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class QuestGiver : MonoBehaviour
{
    [Header("Quest Data")]
    public List<Quest> questsOffered = new List<Quest>();

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

    [Header("No Quest Message")]
    public TextMeshProUGUI noQuestText;

    private Quest selectedQuest;

    public void OpenQuestPanel()
    {
        acceptButton.gameObject.SetActive(false);
        OpenQuestList(questsOffered);
    }

    public void OpenQuestList(List<Quest> quests)
    {
        backgroundQuest.SetActive(true);
        noQuestText.gameObject.SetActive(false);

        // Clear old entries
        foreach (Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }

        int count = 0;

        // Populate list
        foreach (Quest quest in quests)
        {
            // Skip already accepted quests
            if (questManager.activeQuests.Exists(q => q.questName == quest.questName))
                continue;

            count++;

            Quest localQuest = quest;

            GameObject entry = Instantiate(questEntryPrefab, questListContent);
            TextMeshProUGUI entryText = entry.GetComponentInChildren<TextMeshProUGUI>();
            entryText.text = localQuest.questName;

            Button button = entry.GetComponent<Button>();
            button.onClick.AddListener(() => ShowQuestDetails(localQuest));
        }

        if (count == 0)
        {
            noQuestText.text = "No quest available";
            noQuestText.gameObject.SetActive(true);
        }
    }

    private void ShowQuestDetails(Quest quest)
    {
        selectedQuest = quest;

        questNameText.text = quest.questName;
        levelRequirementText.text = $"Level Requirement: {quest.levelRequirement}";

        string obj = "";
        foreach (var o in quest.objectives)
        {
            obj += $"- {o.objectiveName}: {o.currentAmount}/{o.requiredAmount}\n";
        }
        objectivesText.text = obj;

        LevelSystem levelSystem = player.GetComponent<LevelSystem>();
        bool canAccept = levelSystem != null && levelSystem.level >= quest.levelRequirement;

        acceptButton.interactable = canAccept;
        acceptButton.onClick.RemoveAllListeners();

        if (canAccept)
        {
            acceptButton.gameObject.SetActive(true);
            acceptButton.onClick.RemoveAllListeners();
            acceptButton.onClick.AddListener(() =>
            {
                questManager.AcceptQuest(quest);

                // 🧹 Clear detail panel
                selectedQuest = null;
                questNameText.text = "";
                levelRequirementText.text = "";
                objectivesText.text = "";
                acceptButton.gameObject.SetActive(false);

                // 🔁 Refresh the list (quest will no longer appear)
                OpenQuestList(questsOffered);
            });
        }
        else
        {
            acceptButton.gameObject.SetActive(false);
        }
    }

    public void ClosePanel()
    {
        backgroundQuest.SetActive(false);

        // 🧹 Clear selected quest + detail panel
        selectedQuest = null;
        questNameText.text = "";
        levelRequirementText.text = "";
        objectivesText.text = "";
        acceptButton.gameObject.SetActive(false);
    }
}