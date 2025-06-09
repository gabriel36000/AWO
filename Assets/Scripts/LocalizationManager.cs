using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    private Dictionary<string, string> localizedText;
    private string currentLanguage = "en";
    public TMP_FontAsset defaultFont;
    public TMP_FontAsset chineseFont;

    public TMP_FontAsset CurrentFont { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLanguage(currentLanguage); // Load English by default
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLanguage(string langCode)
    {
        currentLanguage = langCode;

        TextAsset langFile = Resources.Load<TextAsset>($"Json/Language/{langCode}");
        if (langFile == null)
        {
            Debug.LogError($"Language file not found: {langCode}");
            return;
        }

        localizedText = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(langFile.text);

        // Set appropriate font
        switch (langCode)
        {
            case "zh":
                CurrentFont = chineseFont;
                break;
            default:
                CurrentFont = defaultFont;
                break;
        }

        Debug.Log($"Language {langCode} loaded with {localizedText.Count} entries.");
    }

    public string GetText(string key)
    {
        return localizedText != null && localizedText.ContainsKey(key) ? localizedText[key] : key;
    }
}
