using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalizationLoader
{
    public static Dictionary<string, string> LoadJSON(string langCode)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Json/Language/{langCode}");

        if (jsonFile == null)
        {
            Debug.LogError($"❌ Language file not found: {langCode}");
            return new Dictionary<string, string>();
        }

        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile.text);
        return dict ?? new Dictionary<string, string>();
    }
}
