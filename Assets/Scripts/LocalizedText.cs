using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    public string localizationKey;
    private TMP_Text textComponent;

    void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        LocalizationUpdater.Register(this);
    }

    public void Refresh()
    {
        if (textComponent != null)
        {
            textComponent.text = LocalizationManager.Instance.GetText(localizationKey);
        }
    }

    void OnDestroy()
    {
        LocalizationUpdater.Unregister(this);
    }
}
