using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;

public class AchievementManager : MonoBehaviour
{
    public List<Achievement> achievements = new List<Achievement>();
    public GameObject achievementPopUp;
    public Canvas mainCanvas;

    public void UnlockAchievement(string title)
    {
        Achievement ach = achievements.Find(a => a.title == title);
        if (ach != null && !ach.unlocked)
        {
            ach.unlocked = true;
            Debug.Log("Achievement Unlocked: " + ach.title);
            ShowPopup(ach);
        }
    }

    void ShowPopup(Achievement ach)
    {
        GameObject popup = Instantiate(achievementPopUp, mainCanvas.transform);
        popup.GetComponentInChildren<TextMeshProUGUI>().text = "Achievement Unlocked: " + ach.title;

        // Optional: auto-destroy after a few seconds
        Destroy(popup, 2.5f);
    }
}
