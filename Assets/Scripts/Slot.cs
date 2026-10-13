using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ItemData data;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text quantity;
    
    public Action<string, string> OnInfoSelected;
    public Action<Slot, Sprite> OnSlotClicked;
    public Action OnSlotReleased;
    
    private bool _isDragging = false;
    
    private void Start()
    {
        icon.sprite = data.icon;
        UpdateQuantity();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnInfoSelected?.Invoke(data.itemName, data.description);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isDragging) return;
        _isDragging = true;
        OnSlotClicked?.Invoke(this, data.icon);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
        OnSlotReleased?.Invoke();
    }
    
    public void SetData(ItemData newData)
    {
        data = newData;
    }

    public void SetIcon(Sprite newImage)
    {
        icon.sprite = newImage;
        icon.color = Color.white;
    }

    public void UpdateQuantity(bool oneItem = false)
    {
        if (oneItem)
        {
            quantity.text = "1";
            print("One Item");
            return;
        }
        
        quantity.text = data.quantity.ToString();
    }

    public ItemData GetData()
    {
        return data;
    }
}
