using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private Transform gridSlots;
    [SerializeField] private Transform dropSlots;
    
    [Header("Visuals")]
    [SerializeField] private RectTransform cursorSlot;
    [SerializeField] private TMP_Text itemInfo;

    [Header("Prefabs")]
    [SerializeField] private Slot slotPrefab;
    
    [Header("Inventory")]
    [SerializeField] private List<ItemData> items;

    private Slot _currentSlot;
    
    private void OnEnable()
    {
        cursorSlot.gameObject.SetActive(false);
        
        foreach (var item in items)
        {
            var newSlot = Instantiate(slotPrefab, transform.position, Quaternion.identity, gridSlots);
            if (newSlot)
            {
                newSlot.SetData(item);
            }
        }
        
        foreach (Transform slotItem in gridSlots)
        {
            if (slotItem.TryGetComponent<Slot>(out var slot))
            {
                slot.OnInfoSelected += OnSlotHover;
                slot.OnSlotClicked += OnSlotClicked;
                slot.OnSlotReleased += OnSlotReleased;
            }
        }
    }

    private void Update()
    {
        //if (!_currentSlot) return;
        var mousePosition = Mouse.current.position.ReadValue();
        cursorSlot.position = mousePosition;
        
        //RectTransformUtility.ScreenPointToLocalPointInRectangle();
        //rectTransform.localPosition = localPoint;
    }
    
    private void OnSlotReleased()
    {
        cursorSlot.gameObject.SetActive(false);
        
        if (_currentSlot.gameObject.TryGetComponent<CanvasGroup>(out var canvasGroup))
        {
            canvasGroup.alpha = 1.0f;
        }

        foreach (Transform drop in dropSlots)
        {
            var rect = drop.GetComponent<RectTransform>();
            var hasChild = drop.childCount > 0;
            
            if (!hasChild && rect && RectTransformUtility.RectangleContainsScreenPoint(rect, cursorSlot.position))
            {
                _currentSlot.transform.SetParent(rect);
            }
        }

        _currentSlot = null;
    }

    private void OnSlotClicked(Slot clickedSlot, Sprite iconSprite)
    {
        cursorSlot.gameObject.SetActive(true);
        
        _currentSlot = clickedSlot;
        
        if (clickedSlot.gameObject.TryGetComponent<CanvasGroup>(out var canvasGroup))
        {
            canvasGroup.alpha = 0.2f;
        }
        
        if (cursorSlot.TryGetComponent<Slot>(out var slot))
        {
            slot.SetIcon(iconSprite);
        }
    }

    private void OnSlotHover(string itemName, string itemDescription)
    {
        itemInfo.text = $"<color=yellow>Nombre:</color> {itemName} | <color=yellow>Descripción:</color> {itemDescription}";
    }

    private void OnDisable()
    {
        foreach (Transform slotItem in gridSlots)
        {
            if (slotItem.TryGetComponent<Slot>(out var slot))
            {
                slot.OnInfoSelected -= OnSlotHover;
                slot.OnSlotClicked -= OnSlotClicked;
                slot.OnSlotReleased -= OnSlotReleased;
            }
        }
    }
}
