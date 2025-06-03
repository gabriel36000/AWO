using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalizationUpdater
{
    private static List<LocalizedText> registeredTexts = new List<LocalizedText>();

    public static void Register(LocalizedText lt)
    {
        if (!registeredTexts.Contains(lt)) registeredTexts.Add(lt);
    }

    public static void Unregister(LocalizedText lt)
    {
        registeredTexts.Remove(lt);
    }

    public static void RefreshAll()
    {
        foreach (var lt in registeredTexts)
        {
            lt.Refresh();
        }
    }
}
