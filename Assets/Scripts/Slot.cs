using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private ItemData data;
    [SerializeField] private Image icon;
    
    public Action<string, string> OnInfoSelected;
    
    private void Start()
    {
        icon.sprite = data.icon;
        if (data.collected)
        {
            icon.color = Color.red;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("Entre al slot");
        OnInfoSelected?.Invoke(data.itemName, data.description);
    }
}
