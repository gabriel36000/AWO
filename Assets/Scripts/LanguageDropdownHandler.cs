using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageDropdownHandler : MonoBehaviour
{
    public TMP_Dropdown languageDropdown;

    void Start()
    {
        // Optional: Set dropdown to current language index
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
    }

    void OnLanguageChanged(int index)
    {
        string selectedLanguage = "en"; // default

        switch (index)
        {
            case 0: selectedLanguage = "en"; break;
            case 1: selectedLanguage = "zh"; break;
                // Add more cases for other languages
        }

        LocalizationManager.Instance.LoadLanguage(selectedLanguage);

        // Optional: Notify UI to refresh text
        LocalizationUpdater.RefreshAll();
    }
}
