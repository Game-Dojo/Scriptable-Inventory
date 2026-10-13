using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public enum QuantityType
    {
        One,
        Half,
        All
    }
    
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

    [Header("Input Actions")] [SerializeField]
    private InputActionReference allItemsAction;
    [SerializeField]
    private InputActionReference halfItemsAction;
    [SerializeField]
    private InputActionReference oneItemAction;

    private Slot _currentSlot;
    private QuantityType _quantityType = QuantityType.All;
    
    private void OnEnable()
    {
        cursorSlot.gameObject.SetActive(false);
        
        foreach (var item in items)
        {
            CreateItem(item, gridSlots);
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

        allItemsAction.action.performed += OnAllItemsAction;
        halfItemsAction.action.performed += OnHalfItemsAction;
        oneItemAction.action.performed += OnOneItemAction;
    }

    private Slot CreateItem(ItemData data, Transform parentObject)
    {
        var newSlot = Instantiate(slotPrefab, transform.position, Quaternion.identity, parentObject);
        if (newSlot) newSlot.SetData(data);
        
        if (_quantityType == QuantityType.One)
            newSlot.UpdateQuantity(true);
        else
            newSlot.UpdateQuantity();
        
        return newSlot;
    }

    private void OnOneItemAction(InputAction.CallbackContext obj)
    {
        _quantityType = QuantityType.One;
    }

    private void OnHalfItemsAction(InputAction.CallbackContext obj)
    {
        _quantityType = QuantityType.Half;
    }

    private void OnAllItemsAction(InputAction.CallbackContext obj)
    {
        _quantityType = QuantityType.All;
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
                var slotData = _currentSlot.GetData();
                
                if (slotData.quantity > 1)
                {
                    Slot item;
                    
                    switch (_quantityType)
                    {
                        case QuantityType.One:
                            slotData.ModifyQuantityByOne();
                            _currentSlot.UpdateQuantity();
                            
                            item = CreateItem(slotData, drop);
                            if (item)
                            {
                                item.UpdateQuantity(true);
                                if (item.TryGetComponent<Slot>(out var slot))
                                {
                                    slot.OnInfoSelected += OnSlotHover;
                                    slot.OnSlotClicked += OnSlotClicked;
                                    slot.OnSlotReleased += OnSlotReleased;
                                }
                            }
                            break;
                        
                        case QuantityType.Half:
                            slotData.ModifyQuantityByHalf();
                            _currentSlot.UpdateQuantity();
                            
                            item = CreateItem(slotData, drop);
                            if (item)
                            {
                                item.UpdateQuantity();
                                if (item.TryGetComponent<Slot>(out var slot))
                                {
                                    slot.OnInfoSelected += OnSlotHover;
                                    slot.OnSlotClicked += OnSlotClicked;
                                    slot.OnSlotReleased += OnSlotReleased;
                                }
                            }
                            break;
                        
                        case QuantityType.All:
                            _currentSlot.transform.SetParent(rect);
                            break;
                    }
                }
                else
                {
                    _currentSlot.transform.SetParent(rect);
                }
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
        
        allItemsAction.action.performed -= OnAllItemsAction;
        halfItemsAction.action.performed -= OnHalfItemsAction;
        oneItemAction.action.performed -= OnOneItemAction;
    }
}
