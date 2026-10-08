using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ItemData data;
    [SerializeField] private Image icon;
    
    public Action<string, string> OnInfoSelected;
    public Action<Slot, Sprite> OnSlotClicked;
    public Action OnSlotReleased;
    
    private bool _isDragging = false;
    
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
    
    public void SetData( ItemData newData)
    {
        data = newData;
    }

    public void SetImage(Sprite newImage)
    {
        icon.sprite = newImage;
        icon.color = Color.white;
    }
}
