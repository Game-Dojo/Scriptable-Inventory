using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ItemData data;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text quantity;
    
    public Action<string, string> OnInfoSelected;
    public Action<Slot, Sprite> OnSlotClicked;
    public Action OnSlotReleased;
    
    private bool _isDragging = false;
    private int _itemsCount = 0;
    
    #region Events
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
    #endregion
    
    #region Data Management
    public void SetData(ItemData newData)
    {
        data = newData;
        icon.sprite = data.icon;
        _itemsCount = data.quantity;
        
        UpdateQuantity();
    }

    public void SetIcon(Sprite newImage)
    {
        icon.sprite = newImage;
        icon.color = Color.white;
    }

    public void UpdateQuantity()
    {
        quantity.text = _itemsCount.ToString();
    }

    public void ShowOne()
    {
        quantity.text = "1";
        _itemsCount = 1;
    }

    public void SetQuantity(int count)
    {
        quantity.text = count.ToString();
        _itemsCount = count;
    }
    
    public void ModifyQuantityByOne()
    {
        if (_itemsCount > 0) _itemsCount -= 1;
    }
    
    public int ModifyQuantityByHalf()
    {
        var rest = 0;
        
        if (_itemsCount > 1)
        {
            rest = _itemsCount % 2;
            _itemsCount += rest;_itemsCount /= 2;
            
        }

        return rest;
    }
    #endregion

    public ItemData GetData() => data;

    public int GetQuantity() => _itemsCount;
    
    public ItemData.ItemType GetItemType() => data.itemType;
    
}
