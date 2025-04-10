using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] public Image image;
    [SerializeField] ItemTooltip tooltip;
    [SerializeField] private TextMeshProUGUI stackText; // Assign this in the inspector (child Text on the icon)
    public event Action<Item> OnRightClickEvent;

    private Item _item;
    public Item Item
    {
        get { return _item; }
        set
        {
            _item = value;

            if (_item == null)
            {
                image.enabled = false;
                stackText.text = "";
            }
            else
            {
                image.sprite = _item.Icon;
                image.enabled = true;

                
                stackText.text = (_item.isStackable && _item.currentStack > 0)
                    ? _item.currentStack.ToString()
                    : "";
            }
        }
    }



    protected virtual void OnValidate() {
        if(image == null) {
            image = GetComponent<Image>();

            if (tooltip == null)
                tooltip = FindObjectOfType<ItemTooltip>();
        }
    }
    public void OnPointerClick(PointerEventData eventData) {
        if(eventData != null && eventData.button == PointerEventData.InputButton.Right) {
            if (Item != null && OnRightClickEvent != null)
                OnRightClickEvent(Item);
        }
    }
    public void OnPointerEnter(PointerEventData eventData) {
        if (Item is EquippableItem && tooltip != null)
        {
            tooltip.ShowTooltip((EquippableItem)Item);
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            tooltip.HideTooltip();
        }
    }
}
