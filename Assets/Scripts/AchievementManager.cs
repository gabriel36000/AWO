using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;
using System.Linq;

public class AchievementManager : MonoBehaviour
{
    public List<Achievement> achievements = new List<Achievement>();
    public GameObject achievementPopUp;
    public Canvas mainCanvas;
    public Transform achievementListParent; // Content container
    public GameObject achievementRowPrefab; // UI row prefab
    public TextMeshProUGUI achievementCountText;
    public void UnlockAchievement(string title)
    {
        Achievement ach = achievements.Find(a => a.title == title);
        if (ach != null && !ach.unlocked)
        {
            ach.unlocked = true;
            Debug.Log("Achievement Unlocked: " + ach.title);
            ShowPopup(ach);
            PopulateAchievementTable(); // <-- Update the table
            UpdateAchievementCountDisplay();
        }
    }

    void ShowPopup(Achievement ach)
    {
        GameObject popup = Instantiate(achievementPopUp, mainCanvas.transform);
        popup.GetComponentInChildren<TextMeshProUGUI>().text = "Achievement Unlocked: " + ach.title;

        // Optional: auto-destroy after a few seconds
        Destroy(popup, 2.5f);
    }
    public void PopulateAchievementTable()
    {
        foreach (Transform child in achievementListParent)
        {
            Destroy(child.gameObject); // Clear old rows
        }

        foreach (Achievement ach in achievements)
        {
            GameObject row = Instantiate(achievementRowPrefab, achievementListParent);
            TextMeshProUGUI[] texts = row.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = ach.title;
            texts[1].text = ach.unlocked ? "Unlocked" : "Locked";
        }
    }
    void UpdateAchievementCountDisplay()
    {
        int total = achievements.Count;
        int unlocked = achievements.Count(a => a.unlocked);
        achievementCountText.text = $"Achievements: {unlocked}/{total}";
    }
    public void Update()
    {
        PopulateAchievementTable(); // <-- Update the table
        UpdateAchievementCountDisplay();
    }
}
