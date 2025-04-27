using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScreenUI : MonoBehaviour
{
    [Header("UI Setup")]
    public GameObject questScreen; // The whole quest screen panel
    public Transform questListContent; // The Content inside the ScrollView
    public GameObject questEntryPrefab; // Prefab of single quest

    private QuestManager questManager;

    private void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
        questScreen.SetActive(false); // Hide at start
    }

    public void OpenQuestScreen()
    {
        questScreen.SetActive(true);
        RefreshQuestList();
    }

    public void CloseQuestScreen()
    {
        questScreen.SetActive(false);
    }

    public void RefreshQuestList()
    {
        if (questManager == null) return;

        // Clear old quest entries
        foreach (Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }

        // Create a quest entry for each active quest
        foreach (var quest in questManager.activeQuests)
        {
            GameObject questEntryObj = Instantiate(questEntryPrefab, questListContent);
            QuestEntryUI entry = questEntryObj.GetComponent<QuestEntryUI>();

            if (entry != null)
            {
                entry.Setup(quest);
            }
        }
    }
}


